using Scellecs.Morpeh.SourceGenerator.Aspect.Core;
using Scellecs.Morpeh.SourceGenerator.Aspect.Model;

namespace Scellecs.Morpeh.SourceGenerator.Aspect.Templates;

public static class AspectFilterExtensionsTemplate
{
    public static GeneratedFile GenerateFile()
    {
        return new GeneratedFile
        {
            FileName = "AspectFilterExtensions.g.cs",
            Content = """
using Scellecs.Morpeh;

namespace Scellecs.Morpeh.SourceGenerator.Aspect
{
    public static class AspectFilterExtensions
    {
        public static FilterBuilder With<T>(this FilterBuilder filter) where T : struct, IAspect, IFilterExtension
        {
            return filter.Extend<T>();
        }
    }
}
"""
        };
    }
}