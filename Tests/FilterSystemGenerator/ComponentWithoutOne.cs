using Scellecs.Morpeh;
using Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator;
using Tests.FilterSystemGenerator.ANamespace;

namespace Tests.FilterSystemGenerator;

public partial class ComponentWithoutOne : FilterSystem
{
    protected override FilterBuilder GetFilter(FilterBuilder filterBuilder)
    {
        return filterBuilder.Without<A>();
    }

    partial void OnUpdate(Entity entity)
    {
        throw new NotImplementedException();
    }
}