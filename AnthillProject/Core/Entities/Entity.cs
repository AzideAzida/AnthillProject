using AnthillProject.Core.Math;
using SkiaSharp;
using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Utilities;

namespace AnthillProject.Core.Entities;

public abstract class Entity : IEntity{
    private const float PositionRoundingMargin = 0.005f;
    
    public virtual void Init(){}

    //When overriding this, please remember to call the base method!
    public virtual void Simulate(){
        Position += Velocity;
        Velocity /= 2;

        //Checking if the entity has left the chunk.
        if(CurrentChunk != null &&
                Position.x >= CurrentChunk.GetSizeX()
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            ||  Position.y >= CurrentChunk.GetSizeY()
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            ||  Position.y <  0
            ||  Position.x <  0
        ){
            CurrentChunk =
            CurrentChunk.Parent.GetChunk(
                CurrentChunk.SaveCoordinates +
                CurrentChunk.GetPossibleExitChunkRelativeCoordinates(CurrentChunk.SaveCoordinates)
            );
            var chunk = CurrentChunk;

            var x = Position.x;
            var y = Position.y;

            /**/ if(Position.x >= chunk.GetSizeX()) x = MathF.Abs(chunk.GetSizeX() - Position.x);
            else if(Position.x <  0               ) x =           chunk.GetSizeX() + Position.x ;

            /**/ if(Position.y >= chunk.GetSizeY()) y = MathF.Abs(chunk.GetSizeY() - Position.y);
            else if(Position.y <  0               ) y =           chunk.GetSizeY() + Position.y ;

            Position = new Vector2(x, y);
        }
    }
    public virtual SKBitmap GetTexture() => GraphicUtils.ArgbBitmap1x1(255, 0, 255);
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