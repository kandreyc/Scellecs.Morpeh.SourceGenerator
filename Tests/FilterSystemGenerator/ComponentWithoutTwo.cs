using Scellecs.Morpeh;
using Tests.FilterSystemGenerator.ANamespace;
using Tests.FilterSystemGenerator.BNamespace;
using Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator;

namespace Tests.FilterSystemGenerator;

public partial class ComponentWithoutTwo : FilterSystem
{
    protected override FilterBuilder GetFilter(FilterBuilder filterBuilder)
    {
        return filterBuilder.Without<A>().Without<B>();
    }

    partial void OnUpdate(Entity entity)
    {
        throw new NotImplementedException();
    }
}