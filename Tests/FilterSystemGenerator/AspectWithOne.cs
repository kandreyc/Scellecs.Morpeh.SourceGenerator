using Scellecs.Morpeh;
using Scellecs.Morpeh.SourceGenerator.AspectGenerator;
using Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator;
using Tests.FilterSystemGenerator.ENamespace;

namespace Tests.FilterSystemGenerator;

public partial class AspectWithOne : FilterSystem
{
    protected override FilterBuilder GetFilter(FilterBuilder filterBuilder)
    {
        return filterBuilder.WithAspect<E>();
    }

    partial void OnUpdate(Entity entity, E e)
    {
        throw new NotImplementedException();
    }
}