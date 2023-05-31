namespace Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator.Templates;

public static class PartialImplTemplate
{
    public static GeneratedFile GenerateFile(SystemInfo systemInfo)
    {
        var data = new Data();

        foreach (var filter in systemInfo.Filters.Distinct().OrderByDescending(f => f.IsAspect))
        {
            if (!string.IsNullOrEmpty(filter.Namespace))
            {
                data.Usings.Add($"using {filter.Namespace};");
            }

            data.Properties.Add(filter.IsAspect
                ? $$"""private AspectFactory<{{filter.NamePascalCase}}> _{{filter.NameCamelCase}}Aspect;"""
                : $$"""private Stash<{{filter.NamePascalCase}}> _{{filter.NameCamelCase}}Stash { get; set; }"""
            );

            data.Initializers.Add(filter.IsAspect
                ? $$"""_{{filter.NameCamelCase}}Aspect = World.GetAspectFactory<{{filter.NamePascalCase}}>();"""
                : $$"""_{{filter.NameCamelCase}}Stash = World.GetStash<{{filter.NamePascalCase}}>(){{(filter.IsDisposable ? ".AsDisposable()" : string.Empty)}};"""
            );

            data.Getters.Add(filter.IsAspect
                ? $$"""_{{filter.NameCamelCase}}Aspect.Get(entity)"""
                : $$"""ref _{{filter.NameCamelCase}}Stash.Get(entity)"""
            );

            data.Arguments.Add(filter.IsAspect
                ? $$"""{{filter.NamePascalCase}} {{filter.NameCamelCase}}"""
                : $$"""ref {{filter.NamePascalCase}} {{filter.NameCamelCase}}""");
        }

        return new GeneratedFile
        {
            FileName = string.IsNullOrEmpty(systemInfo.Namespace)
                ? $"{systemInfo.Name}.g.cs"
                : $"{systemInfo.Namespace}.{systemInfo.Name}.g.cs",

            Content = string.IsNullOrEmpty(systemInfo.Namespace)
                ? GenerateWithoutNameSpace(systemInfo, data)
                : GenerateWithNameSpace(systemInfo, data)
        };
    }

    private static string GenerateWithNameSpace(SystemInfo info, Data data)
    {
        return $$"""
using Scellecs.Morpeh;
using System.Runtime.CompilerServices;
{{GetUsings(data.Usings)}}

namespace {{info.Namespace}}
{
    public partial class {{info.Name}}
    {
        {{GetProperties(data.Properties, indent: 2)}}

        protected override void Initialize()
        {
            {{GetInitializers(data.Initializers, indent: 3)}}
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void Execute(Entity entity)
        {
            OnUpdate(
                entity{{GetGetters(data.Getters, 4)}}
            );
        }

        partial void OnUpdate(Entity entity{{GetArguments(data.Arguments)}});
    }
}
""";
    }

    private static string GenerateWithoutNameSpace(SystemInfo info, Data data)
    {
        return $$"""
using Scellecs.Morpeh;
using System.Runtime.CompilerServices;
{{GetUsings(data.Usings)}}

public partial class {{info.Name}}
{
    {{GetProperties(data.Properties, indent: 1)}}

    protected override void Initialize()
    {
        {{GetInitializers(data.Initializers, indent: 2)}}
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void Execute(Entity entity)
    {
        OnUpdate(
            entity{{GetGetters(data.Getters, indent: 3)}}
        );
    }

    partial void OnUpdate(Entity entity{{GetArguments(data.Arguments)}});
}
""";
    }

    private static string GetIndent(int indent)
    {
        const string tab = "    ";
        return string.Concat(Enumerable.Repeat(tab, indent));
    }

    private static string GetUsings(IReadOnlyCollection<string> usings)
    {
        return string.Join("\n", usings.Distinct());
    }

    private static string GetProperties(IReadOnlyCollection<string> properties, int indent)
    {
        return string.Join($"\n{GetIndent(indent)}", properties);
    }

    private static string GetInitializers(IReadOnlyCollection<string> initializers, int indent)
    {
        return string.Join($"\n{GetIndent(indent)}", initializers);
    }

    private static string GetGetters(IReadOnlyCollection<string> getters, int indent)
    {
        var separator = $",\n{GetIndent(indent)}";

        return getters.Count > 0
            ? string.Join(separator, getters).Insert(0, separator)
            : string.Empty;
    }

    private static string GetArguments(IReadOnlyCollection<string> arguments)
    {
        return arguments.Count > 0
            ? string.Join(", ", arguments).Insert(0, ", ")
            : string.Empty;
    }

    private struct Data
    {
        public List<string> Usings { get; } = new();
        public List<string> Getters { get; } = new();
        public List<string> Arguments { get; } = new();
        public List<string> Properties { get; } = new();
        public List<string> Initializers { get; } = new();

        public Data() { }
    }
}