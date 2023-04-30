using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Scellecs.Morpeh.SourceGenerator.Aspect.Core;
using Scellecs.Morpeh.SourceGenerator.Aspect.Core.Extensions;
using Scellecs.Morpeh.SourceGenerator.Aspect.Model;
using Scellecs.Morpeh.SourceGenerator.Aspect.Templates;

namespace Scellecs.Morpeh.SourceGenerator.Aspect;

[Generator]
public class AspectGenerator : IncrementalGenerator<AspectData>
{
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

    protected override AspectData Select(ISymbol symbol)
    {
        var typeSymbol = (INamedTypeSymbol)symbol;
        var aspectInterface = typeSymbol.Interfaces.First(IsAspectInterface);

        var arguments = aspectInterface.TypeArguments.Select(static p => new Argument
            {
                NamePascalCase = p.Name,
                Type = p.ToDisplayString(),
                Namespace = p.GetNamespaceName(),
                NameCamelCase = p.Name.FirstLetterLowerCase()
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
        const string @namespace = "Scellecs.Morpeh.SourceGenerator.Aspect";

        return symbol is { IsGenericType: true, Name: @name, ContainingNamespace: not null } 
               && symbol.GetNamespaceName() is @namespace;
    }

    protected override void Generate(AspectData aspectData)
    {
        AddSource(AspectTemplate.GenerateFile(aspectData));
    }
}