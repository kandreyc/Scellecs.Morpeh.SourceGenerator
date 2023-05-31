namespace Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator;

[Generator]
public class FilterSystemGenerator : IncrementalGenerator<SystemInfo>
{
    private const string MethodName = "GetFilter";
    private const string DisposableInterface = "System.IDisposable";
    private const string AspectInterface = "Scellecs.Morpeh.IAspect";
    private const string FilterBuilderType = "Scellecs.Morpeh.FilterBuilder";
    private const string FilterSystemType = "Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator.FilterSystem";

    private static readonly HashSet<string> WithMethodNames = new() { "With", "Extend", "WithAspect" };

    protected override void OnPostInitialize()
    {
        AddPostInitializeSource(FilterSystemTemplate.GenerateFile());
    }

    protected override bool SyntaxFilter(SyntaxNode node, CancellationToken _)
    {
        return node is MethodDeclarationSyntax { Parent: ClassDeclarationSyntax classDeclarationSyntax }
               && classDeclarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword)
               && !classDeclarationSyntax.Modifiers.Any(SyntaxKind.AbstractKeyword);
    }

    protected override bool Filter(ISymbol? symbol)
    {

        return symbol is IMethodSymbol methodSymbol 
               && methodSymbol.ContainingType?.BaseType?.ToDisplayString() is FilterSystemType
               && IsGetFilterMethod(methodSymbol);
    }

    private static bool IsGetFilterMethod(IMethodSymbol method)
    {

        return method is { Name: MethodName, Parameters.Length: 1 } 
               && method.ReturnType.ToDisplayString() is FilterBuilderType 
               && method.Parameters[0].Type.ToDisplayString() is FilterBuilderType;
    }

    protected override SystemInfo Select(SyntaxNode methodSyntax, ISymbol symbol, SemanticModel model)
    {
        var methodSymbol = (IMethodSymbol)symbol;
        var @class = methodSymbol.ContainingType;

        return new SystemInfo
        {
            Name = @class.Name,
            Namespace = @class.GetNamespaceName(),
            Filters = GetFilters(methodSyntax, model).ToArray()
        };
    }

    private static IEnumerable<FilterInfo> GetFilters(SyntaxNode methodSyntax, SemanticModel model)
    {
        foreach (var componentType in GetAllTypeArgumentsWithIdentifier(WithMethodNames, methodSyntax, model))
        {
            yield return new FilterInfo
            {
                NamePascalCase = componentType.Name,
                NameCamelCase = componentType.Name.FirstLetterLowerCase(),
                Namespace = componentType.GetNamespaceName(),
                IsAspect = componentType.AllInterfaces.Any(i => i.ToDisplayString() is AspectInterface),
                IsDisposable = componentType.AllInterfaces.Any(i => i.ToDisplayString() is DisposableInterface)
            };
        }
    }

    private static IEnumerable<ITypeSymbol> GetAllTypeArgumentsWithIdentifier(HashSet<string> identifiers, SyntaxNode methodSyntax, SemanticModel model)
    {
        foreach (var syntax in methodSyntax.DescendantNodes())
        {
            if (syntax is not GenericNameSyntax genericNameSyntax) continue;
            if (!identifiers.Contains(genericNameSyntax.Identifier.Text)) continue;
            if (genericNameSyntax.TypeArgumentList.Arguments.Count is not 1) continue;

            var typeArgument = genericNameSyntax.TypeArgumentList.Arguments[0];
            var type = model.GetTypeInfo(typeArgument).Type;

            if (type is not null)
            {
                yield return type;
            }
        }
    }

    protected override void Generate(SystemInfo systemInfo)
    {
        // if (systemInfo.Filters.Length is not 0)
        {
            AddSource(PartialImplTemplate.GenerateFile(systemInfo));
        }
    }
}