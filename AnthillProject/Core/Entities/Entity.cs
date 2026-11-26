using AnthillProject.Core.Math;
using SkiaSharp;
using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Interfaces;
using AnthillProject.Core.Providing;

namespace AnthillProject.Core.Entities;

public abstract class Entity : IProvidable, ISimulable, IInitializable, ITextured{
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
        ArbitraryData = i.Arbitrary;
    }
    //1 unit in the position is 1 tile.
    public Vector2 Position;

    //Velocity is in tiles per tick. Velocity is halved every tick to simulate friction.
    public Vector2 Velocity;

    //Arbitrary data that you can write anything into.
    //After the entity gets saved, this will get saved too and will get restored after the entity gets loaded again.
    public byte[] ArbitraryData{get; set;} = [];
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