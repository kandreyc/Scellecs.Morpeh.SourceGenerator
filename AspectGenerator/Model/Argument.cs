namespace Scellecs.Morpeh.SourceGenerator.Aspect.Model;

public struct Argument
{
    public string Type { get; set; }
    public string? Namespace { get; set; }
    public string NameCamelCase { get; set; }
    public string NamePascalCase { get; set; }
}