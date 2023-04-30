namespace Scellecs.Morpeh.SourceGenerator.Aspect.Core;

public interface ISourceFileGenerator
{
    void AddSource(GeneratedFile file);
    void AddPostInitializeSource(GeneratedFile file);
}