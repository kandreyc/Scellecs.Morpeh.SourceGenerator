using System.Linq;
using Scellecs.Morpeh.SourceGenerator.Aspect.Core;
using Scellecs.Morpeh.SourceGenerator.Aspect.Model;

namespace Scellecs.Morpeh.SourceGenerator.Aspect.Templates;

public static class AspectTemplate
{
    private static string Stash(string name) => $"_{name}Stash";
    private static string Tab(int indent) => string.Concat(Enumerable.Repeat("    ", indent));

    public static GeneratedFile GenerateFile(AspectData data)
    {
        return new GeneratedFile
        {
            FileName = GetName(),
            Content = data.Namespace is not null ? GetWithNamespace() : GetWithoutNamespace()
        };

        string GetWithNamespace()
        {
            return $$"""
using Scellecs.Morpeh;{{GetUsings()}}

namespace {{data.Namespace}}
{
    public partial struct {{data.Name}}
    {
        {{GetStashes(2)}}

        public Entity Entity { get; set; }

        {{GetProperties(2)}}

        void IAspect.OnGetAspectFactory(World world)
        {
            {{GetCtors(3)}}
        }

        FilterBuilder IFilterExtension.Extend(FilterBuilder rootFilter)
        {
            return rootFilter{{GetFilters()}};
        }
    }
}
""";
        }

        string GetWithoutNamespace()
        {
            return $$"""
using Scellecs.Morpeh;{{GetUsings()}}

public partial struct {{data.Name}}
{
    {{GetStashes(1)}}

    public Entity Entity { get; set; }

    {{GetProperties(1)}}

    void IAspect.OnGetAspectFactory(World world)
    {
        {{GetCtors(2)}}
    }

    FilterBuilder IFilterExtension.Extend(FilterBuilder rootFilter)
    {
        return rootFilter{{GetFilters()}};
    }
}
""";
        }

        string GetStashes(int indent)
        {
            var str = data.Arguments.Select(
                static a => $"private Stash<{a.NamePascalCase}> {Stash(a.NameCamelCase)};"
            );

            return string.Join($"\n{Tab(indent)}", str);
        }

        string GetProperties(int indent)
        {
            var str = data.Arguments.Select(
                static a => $"public ref {a.NamePascalCase} {a.NamePascalCase} => ref {Stash(a.NameCamelCase)}.Get(Entity);"
            );

            return string.Join($"\n{Tab(indent)}", str);
        }

        string GetCtors(int indent)
        {
            var str = data.Arguments.Select(
                static a => $"{Stash(a.NameCamelCase)} = world.GetStash<{a.NamePascalCase}>();"
            );

            return string.Join($"\n{Tab(indent)}", str);
        }

        string GetFilters()
        {
            return string.Concat(data.Arguments.Select(static a => $".With<{a.NamePascalCase}>()"));
        }

        string GetName()
        {
            return data.Namespace is null
                ? $"{data.Name}.g.cs"
                : $"{data.Namespace}.{data.Name}.g.cs";
        }

        string GetUsings()
        {
            var str = data.Arguments
                .Where(a => a.Namespace is not null && a.Namespace != data.Namespace)
                .Select(static a => $"using {a.Namespace};")
                .Distinct()
                .ToArray();

            return str.Length is 0 ? string.Empty : $"\n{string.Join("\n", str)}";
        }
    }
}