using Scellecs.Morpeh;
using Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator;
using Tests.FilterSystemGenerator.ANamespace;
using Tests.FilterSystemGenerator.BNamespace;

namespace Tests.FilterSystemGenerator;

public partial class ComponentWithTwo : FilterSystem
{
    protected override FilterBuilder GetFilter(FilterBuilder filterBuilder)
    {
        return filterBuilder.With<A>().With<B>();
    }

    partial void OnUpdate(Entity entity, ref A a, ref B b)
    {
        throw new NotImplementedException();
    }
}