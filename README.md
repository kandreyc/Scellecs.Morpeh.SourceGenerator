## Aspects

Just use a ```IAspect<>``` with specified components and aspect boilerplate code will be generated for you.

```cs
public struct Scale : IComponent
{
    public Vector3 Value;
}

public struct Position : IComponent
{
    public Vector3 Value;
}

public struct Rotation : IComponent
{
    public Vector3 Value;
}

public partial struct Transform : IAspect<Position, Rotation, Scale> { }
```

Generator will provide the following:

```cs
public partial struct Transform
{
    private Stash<Position> _positionStash;
    private Stash<Rotation> _rotationStash;
    private Stash<Scale> _scaleStash;

    public Entity Entity { get; set; }

    public ref Position Position => ref _positionStash.Get(Entity);
    public ref Rotation Rotation => ref _rotationStash.Get(Entity);
    public ref Scale Scale => ref _scaleStash.Get(Entity);

    void IAspect.OnGetAspectFactory(World world)
    {
        _positionStash = world.GetStash<Position>();
        _rotationStash = world.GetStash<Rotation>();
        _scaleStash = world.GetStash<Scale>();
    }

    FilterBuilder IFilterExtension.Extend(FilterBuilder rootFilter)
    {
        return rootFilter.With<Position>().With<Rotation>().With<Scale>();
    }
}
```
