using AnthillProject.Core.Math;
using AnthillProject.Core.Providing;

namespace AnthillProject.Core.Entities;

public readonly struct EntityProviderArgs : IInstanceProviderArgs<Entity>{
    public readonly Vector2 Position, Velocity;
    public readonly byte[] Arbitrary;
    public EntityProviderArgs(): this(new Vector2()){}
    public EntityProviderArgs(Vector2 position): this(position, new Vector2()){}
    public EntityProviderArgs(Vector2 position, Vector2 velocity): this(position, velocity, []){}
    public EntityProviderArgs(Vector2 position, Vector2 velocity, byte[] arbitrary){
        Position = position;
        Velocity = velocity;
        Arbitrary = arbitrary;
    }
    public byte[] GetArbitraryData() => Arbitrary;
}