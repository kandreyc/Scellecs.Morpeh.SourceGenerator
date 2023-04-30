using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Scellecs.Morpeh.SourceGenerator.Aspect.Core.Extensions;

internal static class AttributeDataExtensions
{
    public static string? GetNamedParamValue(this AttributeData attributeData, string paramName)
    {
        var pair = attributeData.NamedArguments.FirstOrDefault(x => x.Key == paramName);
        return pair.Value.Value?.ToString();
    }

    public static bool TryGetAttribute(this ImmutableArray<AttributeData> attributes, string type, out AttributeData attribute)
    {
        var attr = attributes.FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == type);

        if (attr is null)
        {
            attribute = default!;
            return false;
        }

        attribute = attr;
        return true;
    }
}