namespace Scellecs.Morpeh.SourceGenerator.Core.Extensions;

// ReSharper disable once InconsistentNaming
public static class ITypeSymbolExtensions
{
    public static string? GetNamespaceName(this ITypeSymbol symbol)
    {
        return symbol.ContainingNamespace.GetNamespaceName();
    }
}