### IAspect<>

Generic ```IAspect``` version with an ability to specify components and all required aspect implementation will be generated for you.

```cs
public struct Position : IComponent  
{  
    public Vector3 Value;  
}  

public struct Rotation : IComponent  
{  
    public Vector3 Value;  
}

public struct Scale : IComponent  
{  
    public Vector3 Value;  
}

public partial struct Transform : IAspect<Position, Rotation, Scale> { }

// Generated
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

---

### FilterSystem

FilterSystem Generator is inspired by Unity's JobSystem generation that allows dynamic  arguments in `Execute()` method. Filter system allows to specify which entities you want to iterate on during the update, with dynamic arguments in `OnUpdate()` method.

Lets see it in action:

We have a `Position` and `Velocity` components

```cs
public struct Position : IComponent
{
    public Vector3 Value;
}

public struct Velocity : IComponent
{
    public Vector3 Value;
}
```

And we want to have a system that will operate with those entities.

What we need is to nest `FilterSystem` and specify `partial` modifier.

`FilterSystem` is a generated base class, that asks you to implement `GetFilter()` method, that should contain your filter build query.

```cs
public partial class MoveSystem : FilterSystem
{
    protected override FilterBuilder GetFilter(FilterBuilder builder)
    {
        return builder.With<Position>().With<Velocity>();
    }
}
```

Based on it, partial `OnUpdate()` method declaration will be generated with a direct ref access to each component.

```cs
// Generated
public partial class MoveSystem
{
    // ...
    partial void OnUpdate(Entity entity, ref Position position, ref Velocity velocity);
}
```

And then you can just implement that `OnUpdate()` method on your side.

```cs
partial void OnUpdate(Entity entity, ref Position position, ref Velocity velocity)
{
    position.Value += velocity.Value * Time.deltaTime;
}
```

#### Lets Compare!

Example of generated version:

```cs
public partial class MoveSystem : FilterSystem
{
    protected override FilterBuilder GetFilter(FilterBuilder builder)
    {
        return builder.With<Position>().With<Velocity>();
    }

    partial void Execute(Entity entity, ref Position position, ref Velocity velocity)
    {
        position.Value += velocity.Value * Time.deltaTime;
    }
}
```

Manually written system:

1. Default version

```cs
public class MoveSystem : ISystem
{
    private Filter _filter;

    public World World { get; set; }

    public void OnAwake()
    {
        _filter = World.Filter.With<Position>().With<Velocity>().Build();
    }

    public void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter)
        {
            ref var position = ref entity.GetComponent<Position>();
            ref var velocity = ref entity.GetComponent<Position>();

            position.Value += velocity.Value * deltaTime;
        }
    }
}
```

2. More efficient version

```cs
public class MoveSystem : ISystem
{
    private Filter _filter;
    private Stash<Position> _positionStash;
    private Stash<Velocity> _velocityStash;

    public World World { get; set; }

    public void OnAwake()
    {
        _positionStash = World.GetStash<Position>();
        _velocityStash = World.GetStash<Velocity>();
        _filter = World.Filter.With<Position>().With<Velocity>().Build();
    }

    public void OnUpdate(float deltaTime)
    {
        foreach (var entity in _filter)
        {
            ref var position = ref _positionStash.Get(entity);
            ref var velocity = ref _velocityStash.Get(entity);

            position.Value += velocity.Value * deltaTime;
        }
    }
}
```

#### Important notes

Generated part of your `FilterSystem` gets the components in a more efficient way, directly from stash, not entity.

```cs
// Generated
public partial class MoveSystem
{
    private Stash<Position> _positionStash;
    private Stash<Velocity> _velocityStash;

    protected override void Initialize()
    {
        _positionStash = World.GetStash<Position>();
        _velocityStash = World.GetStash<Velocity>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void Execute(Entity entity)
    {
        OnUpdate(
            entity,
            ref _positionStash.Get(entity),
            ref _velocityStash.Get(entity)
        );
    }
    // ...
}
```

Generator supports `IDisposable` components and marks the `Stash` if it is required.

```cs
public struct View : IComponent, IDisposable
{
    public GameObject Value;
    // ...
}

// Generated
public partial class DestroySystem
{
    private Stash<View> _viewStash;

    protected override void Initialize()
    {
        _viewStash = World.GetStash<View>().AsDisposable();
    }
    // ...
}
```

Generator supports core `IAspect` and. generic `IAspect<>`

```cs
// generic IAspect from AspectGenerator or manually written IAspect
public partial struct Transform : IAspect<Position, Rotation, Scale> { }

public struct Position : IComponent
{
    public Vector3 Value;
}

public struct Rotation : IComponent
{
    public Vector3 Value;
}

public struct Scale : IComponent
{
    public Vector3 Value;
}

public struct Velocity : IComponent
{
    public Vector3 Value;
}

public partial class MoveSystem : FilterSystem
{
    protected override FilterBuilder GetFilter(FilterBuilder builder)
    {
        // There are also extension 'WithAspect<>' instead of 'Extend'
        return builder.With<Velocity>().Extend<Transform>();
    }

    partial void OnUpdate(Entity entity, Transform transform, ref Velocity velocity)
    {
        transform.Position.Value += velocity.Value * Time.deltaTime;
    }
}


// Generated
public partial class TransformSystem
{
    private AspectFactory<Transform> _transformAspect;
    private Stash<Velocity> _velocityStash;

    protected override void Initialize()
    {
        _transformAspect = World.GetAspectFactory<Transform>();
        _velocityStash = World.GetStash<Velocity>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void Execute(Entity entity)
    {
        OnUpdate(
            entity,
            _transformAspect.Get(entity),
            ref _velocityStash.Get(entity)
        );
    }

    partial void OnUpdate(Entity entity, Transform transform, ref Velocity velocity);
}
```