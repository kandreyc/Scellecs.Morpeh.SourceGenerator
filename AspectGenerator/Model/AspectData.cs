namespace Scellecs.Morpeh.SourceGenerator.Aspect.Model;

public struct AspectData
{
    public string Name { get; set; }
    public string? Namespace { get; set; }
    public Argument[] Arguments { get; set; }
}