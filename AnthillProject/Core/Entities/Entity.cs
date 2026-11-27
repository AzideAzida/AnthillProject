using AnthillProject.Core.Math;
using SkiaSharp;
using AnthillProject.Core.Map.Chunks;

namespace AnthillProject.Core.Entities;

public abstract class Entity : IEntity{
    private const float PositionRoundingMargin = 0.005f;
    
    public virtual void Init(){}

    //When overriding this, please remember to call the base method!
    public virtual void Simulate(){
        Position += Velocity;
        Position = new Vector2(
            Utility.RoundIfCloseToBorders(Position.x, PositionRoundingMargin),
            Utility.RoundIfCloseToBorders(Position.y, PositionRoundingMargin)
        );

        Velocity /= 2;
    }
    public virtual SKBitmap GetTexture() => Utility.ArgbBitmap1x1(255, 0, 255);
    public abstract Vector2 GetSize();
    public abstract string GetProviderIdentifier();
    public Entity(){}
    public Entity(EntityProviderArgs i){
        Position = i.Position;
        Velocity = i.Velocity;
    }
    //1 unit in the position is 1 tile.
    public Vector2 Position{get; set;}

    //Velocity is in tiles per tick. Velocity is halved every tick to simulate friction.
    public Vector2 Velocity{get; set;}

    //Arbitrary data that can return anything.
    //After the entity gets saved, this will get saved too and will be provided in the constructor in the EntityProviderArgs after the entity gets loaded again.
    public virtual byte[] GetArbitraryData() => [];
    public IChunk? CurrentChunk{
        get;
        set{
            if(field == value)
                return;

            if(field != null)
                field.LoadedEntities.Remove(this);
            
            field = value;
            
            if(field != null)
                field.LoadEntity(this);
        }
    }
}