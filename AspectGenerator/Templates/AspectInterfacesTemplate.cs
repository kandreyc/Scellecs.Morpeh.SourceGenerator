namespace Scellecs.Morpeh.SourceGenerator.AspectGenerator.Templates;

public static class AspectInterfacesTemplate
{
    public static GeneratedFile GenerateFile()
    {
        return new GeneratedFile
        {
            FileName = "IAspect.g.cs",
            Content = """
using Scellecs.Morpeh;

namespace Scellecs.Morpeh.SourceGenerator.AspectGenerator
{
    public interface IAspect<T1, T2> : IAspect, IFilterExtension
        where T1 : struct, IComponent
        where T2 : struct, IComponent { }

    public interface IAspect<T1, T2, T3> : IAspect, IFilterExtension
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent { }

    public interface IAspect<T1, T2, T3, T4> : IAspect, IFilterExtension
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent { }

    public interface IAspect<T1, T2, T3, T4, T5> : IAspect, IFilterExtension
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent { }

    public interface IAspect<T1, T2, T3, T4, T5, T6> : IAspect, IFilterExtension
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent { }

    public interface IAspect<T1, T2, T3, T4, T5, T6, T7> : IAspect, IFilterExtension
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent { }

    public interface IAspect<T1, T2, T3, T4, T5, T6, T7, T8> : IAspect, IFilterExtension
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
        where T8 : struct, IComponent { }

    public interface IAspect<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IAspect, IFilterExtension
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
        where T8 : struct, IComponent
        where T9 : struct, IComponent { }

    public interface IAspect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IAspect, IFilterExtension
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
        where T8 : struct, IComponent
        where T9 : struct, IComponent
        where T10 : struct, IComponent { }
}
"""
        };
    }
}