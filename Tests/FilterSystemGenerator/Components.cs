using Scellecs.Morpeh;

namespace Tests.FilterSystemGenerator.ANamespace
{
    public struct A : IComponent, IDisposable
    {
        public int Value;
        public void Dispose() { }
    }
}

namespace Tests.FilterSystemGenerator.BNamespace
{
    public struct B : IComponent
    {
        public int Value;
    }
}

public struct C : IComponent
{
    public int Value;
}