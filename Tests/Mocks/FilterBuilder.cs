// ReSharper disable once CheckNamespace

namespace Scellecs.Morpeh;

public struct FilterBuilder
{
    public FilterBuilder With<T>() where T: IComponent
    {
        throw new NotImplementedException();
    }

    public FilterBuilder Without<T>() where T: IComponent
    {
        throw new NotImplementedException();
    }

    public FilterBuilder Extend<T>() where T: IFilterExtension
    {
        throw new NotImplementedException();
    }
}