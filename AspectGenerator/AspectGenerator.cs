namespace Scellecs.Morpeh.SourceGenerator.AspectGenerator;

[Generator]
public class AspectGenerator : IncrementalGenerator<AspectData>
{
    private const string DisposableInterface = "System.IDisposable";

    protected override void OnPostInitialize()
    {
        AddPostInitializeSource(AspectInterfacesTemplate.GenerateFile());
        AddPostInitializeSource(AspectFilterExtensionsTemplate.GenerateFile());
    }

    protected override bool SyntaxFilter(SyntaxNode node, CancellationToken _)
    {
        if (node is not StructDeclarationSyntax syntax)
        {
            return false;
        }

        foreach (var modifier in syntax.Modifiers)
        {
            if (modifier.IsKind(SyntaxKind.PartialKeyword))
            {
                return true;
            }
        }

        return false;
    }

    protected override bool Filter(ISymbol? symbol)
    {
        return symbol is INamedTypeSymbol typeSymbol && typeSymbol.Interfaces.Any(IsAspectInterface);
    }

    protected override AspectData Select(SyntaxNode _, ISymbol symbol, SemanticModel __)
    {
        var typeSymbol = (INamedTypeSymbol)symbol;
        var aspectInterface = typeSymbol.Interfaces.First(IsAspectInterface);

        var arguments = aspectInterface.TypeArguments.Select(static p => new Argument
            {
                NamePascalCase = p.Name,
                Type = p.ToDisplayString(),
                Namespace = p.GetNamespaceName(),
                NameCamelCase = p.Name.FirstLetterLowerCase(),
                IsDisposable = p.AllInterfaces.Any(i => i.ToDisplayString() == DisposableInterface)
            })
            .Distinct()
            .ToArray();

        return new AspectData
        {
            Arguments = arguments,
            Name = typeSymbol.Name,
            Namespace = typeSymbol.GetNamespaceName(),
        };
    }

    private static bool IsAspectInterface(INamedTypeSymbol symbol)
    {
        const string @name = "IAspect";
        const string @namespace = "Scellecs.Morpeh.SourceGenerator.AspectGenerator";

        return symbol is { IsGenericType: true, Name: @name, ContainingNamespace: not null } 
               && symbol.GetNamespaceName() is @namespace;
    }

    protected override void Generate(AspectData aspectData)
    {
        AddSource(AspectTemplate.GenerateFile(aspectData));
    }
}