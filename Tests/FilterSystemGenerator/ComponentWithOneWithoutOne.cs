using Scellecs.Morpeh;
using Tests.FilterSystemGenerator.ANamespace;
using Tests.FilterSystemGenerator.BNamespace;
using Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator;

namespace Tests.FilterSystemGenerator;

public partial class ComponentWithOneWithoutOne : FilterSystem
{
    protected override FilterBuilder GetFilter(FilterBuilder filterBuilder)
    {
        return filterBuilder.With<A>().Without<B>();
    }

    partial void OnUpdate(Entity entity, ref A a)
    {
        throw new NotImplementedException();
    }
}