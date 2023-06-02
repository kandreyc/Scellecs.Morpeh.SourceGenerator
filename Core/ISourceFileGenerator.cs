namespace Scellecs.Morpeh.SourceGenerator.Core;

public interface ISourceFileGenerator
{
    void AddSource(GeneratedFile file);
    void AddPostInitializeSource(GeneratedFile file);
}