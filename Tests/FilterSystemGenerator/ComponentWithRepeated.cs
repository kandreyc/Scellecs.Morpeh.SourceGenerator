using Scellecs.Morpeh;
using Tests.FilterSystemGenerator.ANamespace;
using Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator;

namespace Tests.FilterSystemGenerator;

public partial class ComponentWithRepeated : FilterSystem
{
    protected override FilterBuilder GetFilter(FilterBuilder filterBuilder)
    {
        return filterBuilder.With<A>().With<A>();
    }

    partial void OnUpdate(Entity entity, ref A a)
    {
        throw new NotImplementedException();
    }
}