using System;
using System.Text;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;

namespace Scellecs.Morpeh.SourceGenerator.Core;

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
                return Filter(symbol) ? (node: c.Node, symbol, model: c.SemanticModel) : default;
            })
            .Where(t => t.symbol is not null)
            .Select((t, _) => Select(t.node, t.symbol!, t.model));

        context.RegisterSourceOutput(provider.Collect(), GenerateInternal);
    }

    protected abstract TData Select(SyntaxNode syntax, ISymbol symbol, SemanticModel model);

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