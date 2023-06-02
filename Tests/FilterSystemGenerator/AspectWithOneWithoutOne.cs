using Scellecs.Morpeh;
using Scellecs.Morpeh.SourceGenerator.AspectGenerator;
using Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator;
using Tests.FilterSystemGenerator.ANamespace;
using Tests.FilterSystemGenerator.ENamespace;

namespace Tests.FilterSystemGenerator;

public partial class AspectWithOneWithoutOne : FilterSystem
{
    protected override FilterBuilder GetFilter(FilterBuilder filterBuilder)
    {
        return filterBuilder.WithAspect<E>().Without<A>();
    }

    partial void OnUpdate(Entity entity, E e)
    {
        throw new NotImplementedException();
    }
}