namespace Scellecs.Morpeh.SourceGenerator.AspectGenerator.Model;

public struct AspectData
{
    public string Name { get; set; }
    public string? Namespace { get; set; }
    public Argument[] Arguments { get; set; }
}