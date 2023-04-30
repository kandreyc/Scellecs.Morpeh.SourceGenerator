using Microsoft.CodeAnalysis;

namespace Scellecs.Morpeh.SourceGenerator.Aspect.Core.Extensions;

// ReSharper disable once InconsistentNaming
public static class INamespaceSymbolExtensions
{
    public static string? GetNamespaceName(this INamespaceSymbol? symbol)
    {
        return symbol is null || symbol.IsGlobalNamespace ? default : symbol.ToDisplayString();
    }
}