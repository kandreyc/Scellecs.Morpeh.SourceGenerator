namespace Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator.Model;

public struct SystemInfo
{
    public string Name { get; set; }
    public string? Namespace { get; set; }
    public FilterInfo[] Filters { get; set; }
}