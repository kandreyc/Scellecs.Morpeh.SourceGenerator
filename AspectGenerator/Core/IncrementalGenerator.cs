using System;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;

namespace Scellecs.Morpeh.SourceGenerator.Aspect.Core;

public abstract class IncrementalGenerator<TData> : IIncrementalGenerator, ISourceFileGenerator
    where TData: struct
{
    protected Logger.Logger Logger { get; }
    private SourceProductionContext _productionContext;
    private IncrementalGeneratorPostInitializationContext _initializationContext;

    protected IncrementalGenerator()
    {
        Logger = new Logger.Logger();
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(c =>
        {
            _initializationContext = c;
            OnPostInitialize();
        });

        var provider = context.SyntaxProvider
            .CreateSyntaxProvider(SyntaxFilter, (c, _) =>
            {
                var symbol = c.SemanticModel.GetDeclaredSymbol(c.Node);
                return Filter(symbol) ? symbol : null;
            })
            .Where(symbol => symbol is not null)
            .Select((symbol, _) => Select(symbol!));

        context.RegisterSourceOutput(provider.Collect(), GenerateInternal);
    }

    protected abstract TData Select(ISymbol symbol);

    private void GenerateInternal(SourceProductionContext context, ImmutableArray<TData> data)
    {
        if (data.Length is 0)
        {
            return;
        }

        _productionContext = context;

        try
        {
            foreach (var elem in data)
            {
                Generate(elem);
            }
        }
        catch (Exception e)
        {
            Logger.Log(e.ToString());
        }
        finally
        {
            Logger.Flush(AddSource);
        }
    }

    public void AddPostInitializeSource(GeneratedFile file)
    {
        _initializationContext.AddSource(file.FileName, SourceText.From(file.Content, Encoding.UTF8));
    }

    public void AddSource(GeneratedFile file)
    {
        _productionContext.AddSource(file.FileName, SourceText.From(file.Content, Encoding.UTF8));
    }

    protected virtual void OnPostInitialize() { }
    protected abstract bool SyntaxFilter(SyntaxNode node, CancellationToken cancellationToken);
    protected abstract bool Filter(ISymbol? symbol);
    protected virtual void Generate(TData data) { }
}