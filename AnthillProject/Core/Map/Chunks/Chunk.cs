using AnthillProject.Core.Math;
using AnthillProject.Core.Entities;
using AnthillProject.Core.Map.Tiles;
using AnthillProject.Core.Tiling;
using AnthillProject.Core.Server;

namespace AnthillProject.Core.Map.Chunks;
public class Chunk : TiledObject<Tile>, IChunk{
    public override Tile GetDefaultTile() =>
        new UnknownTile(new TileProviderArgs());
    public int GetSizeX() => SizeX;
    public int GetSizeY() => SizeY;

    public Coordinates GetCoordinates(Tile tile) => GetCoordinatesOfTile(tile);

    public Chunk() : this(8, 8){}
    public Chunk(int sx, int sy) : base(sx, sy){
        foreach(var i in GetAllCoordinates())
            this[i].Parent = this;
        Parent = Program.Instance.Server;
    }

    public override void Simulate(){
        base.Simulate();
        foreach(var i in LoadedEntities){
            i.Simulate();

            //Checking if the entity has left the chunk.
            if(
                    i.Position.x >= GetSizeX()
                ||  i.Position.y >= GetSizeY()
                ||  i.Position.y <  0
                ||  i.Position.x <  0
            ) Parent.HandleEntityChunkTransition(i);
        }
    }
    public IServer Parent{get; set;}

    public IList<Entity> LoadedEntities{get; set;} = new List<Entity>();
    
    public Coordinates SaveCoordinates{get; set;}
    
    public void SummonEntity(Entity entity){
        entity.Init();
        LoadEntity(entity);
    }

    public void LoadEntity(Entity entity){
        LoadedEntities.Add(entity);
        entity.CurrentChunk = this;
    }

    public void SetTile(Coordinates coordinates, Tile tile){
        this[coordinates] = tile;
        tile.Parent = this;
    }
}