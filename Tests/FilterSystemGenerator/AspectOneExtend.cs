using Scellecs.Morpeh;
using Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator;
using Tests.FilterSystemGenerator.DNamespace;

namespace Tests.FilterSystemGenerator;

public partial class AspectOneExtend : FilterSystem
{
    protected override FilterBuilder GetFilter(FilterBuilder filterBuilder)
    {
        return filterBuilder.Extend<D>();
    }

    partial void OnUpdate(Entity entity, D d)
    {
        throw new NotImplementedException();
    }
}