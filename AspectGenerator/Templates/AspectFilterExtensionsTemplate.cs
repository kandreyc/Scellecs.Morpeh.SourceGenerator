namespace Scellecs.Morpeh.SourceGenerator.AspectGenerator.Templates;

public static class AspectFilterExtensionsTemplate
{
    public static GeneratedFile GenerateFile()
    {
        const string @namespace = "Scellecs.Morpeh.SourceGenerator.AspectGenerator";

        return new GeneratedFile
        {
            FileName = $"{@namespace}.AspectFilterExtensions.g.cs",
            Content = $$"""
using Scellecs.Morpeh;

namespace {{@namespace}}
{
    public static class AspectFilterExtensions
    {
        public static FilterBuilder WithAspect<T>(this FilterBuilder filter) where T : struct, IAspect, IFilterExtension
        {
            return filter.Extend<T>();
        }
    }
}
"""
        };
    }
}