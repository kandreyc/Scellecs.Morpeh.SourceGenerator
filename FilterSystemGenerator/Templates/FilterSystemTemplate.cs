namespace Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator.Templates;

public static class FilterSystemTemplate
{
    public static GeneratedFile GenerateFile()
    {
        const string @namespace = "Scellecs.Morpeh.SourceGenerator.FilterSystemGenerator";

        return new GeneratedFile
        {
            FileName = $"{@namespace}.FilterSystem.g.cs",
            Content = $$"""
using Scellecs.Morpeh;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace {{@namespace}}
{
    public abstract class FilterSystem : ISystem
    {
        public World World { get; set; }
        private Filter Filter { get; set; }

        public virtual void OnAwake()
        {
            Initialize();
            Filter = GetFilter(World.Filter).Build();
        }

        protected virtual void Initialize() { }

        protected abstract FilterBuilder GetFilter(FilterBuilder builder);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnUpdate(float _)
        {
            Execute(Filter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Execute(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                Execute(entity);
            }
        }

        protected virtual void Execute(Entity entity) { }

        public virtual void Dispose() { }
    }
}
"""
        };
    }
}