namespace Scellecs.Morpeh.SourceGenerator.AspectGenerator.Model;

public struct Argument
{
    public string Type { get; set; }
    public bool IsDisposable { get; set; }
    public string? Namespace { get; set; }
    public string NameCamelCase { get; set; }
    public string NamePascalCase { get; set; }
}