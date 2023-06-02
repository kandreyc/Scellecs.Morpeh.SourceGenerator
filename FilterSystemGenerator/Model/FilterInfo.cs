namespace Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator.Model;

public struct FilterInfo
{
    public string NamePascalCase { get; set; }
    public string NameCamelCase { get; set; }
    public string? Namespace { get; set; }
    public bool IsAspect { get; set; }
    public bool IsDisposable { get; set; }
}