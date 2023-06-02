using Scellecs.Morpeh;
using Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator;

namespace Tests.FilterSystemGenerator;

public partial class EmptyFilter : FilterSystem
{
    protected override FilterBuilder GetFilter(FilterBuilder filterBuilder)
    {
        return filterBuilder;
    }

    partial void OnUpdate(Entity entity)
    {
        throw new NotImplementedException();
    }
}