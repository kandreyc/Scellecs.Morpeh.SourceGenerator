using Scellecs.Morpeh;
using Tests.FilterSystemGenerator.DNamespace;
using Scellecs.Morpeh.SourceGenerator.AspectGenerator;
using Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator;

namespace Tests.FilterSystemGenerator;

public partial class AspectOneExtendOneWithAspect : FilterSystem
{
    protected override FilterBuilder GetFilter(FilterBuilder filterBuilder)
    {
        return filterBuilder.Extend<D>().WithAspect<F>();
    }

    partial void OnUpdate(Entity entity, D d, F f)
    {
        throw new NotImplementedException();
    }
}