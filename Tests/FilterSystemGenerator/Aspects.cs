using Tests.FilterSystemGenerator.ANamespace;
using Tests.FilterSystemGenerator.BNamespace;
using Scellecs.Morpeh.SourceGenerator.AspectGenerator;

namespace Tests.FilterSystemGenerator.DNamespace
{
    public partial struct D : IAspect<A, B> { }
}

namespace Tests.FilterSystemGenerator.ENamespace
{
    public partial struct E : IAspect<C, B> { }
}

public partial struct F : IAspect<B, C> { }