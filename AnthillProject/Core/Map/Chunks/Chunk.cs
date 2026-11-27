using AnthillProject.Core.Math;
using AnthillProject.Core.Entities;
using AnthillProject.Core.Map.Tiles;
using AnthillProject.Core.Tiling;
using AnthillProject.Core.Server;

namespace AnthillProject.Core.Map.Chunks;
public class Chunk : TiledObject<ITile>, IChunk{
    public override ITile GetDefaultTile() =>
        new UnknownTile(new TileProviderArgs());
    public int GetSizeX() => SizeX;
    public int GetSizeY() => SizeY;

    public Coordinates GetCoordinates(ITile tile) => GetCoordinatesOfTile(tile);

    public Chunk() : this(8, 8){}
    public Chunk(int sx, int sy) : base(sx, sy){
        foreach(var i in GetAllCoordinates())
            this[i].Parent = this;
        Parent = Program.Instance.Server;
    }

    public override void Simulate(){
        base.Simulate();
        foreach(var i in LoadedEntities)
            i.Simulate();
    }
    public IServer Parent{get; set;}

    public IList<IEntity> LoadedEntities{get; set;} = new List<IEntity>();
    
    public Coordinates SaveCoordinates{get; set;}
    public void OnUnload(){
        foreach(var i in GetAllCoordinates())
            Utility.TryDispose(GetTile(i.x, i.y));
        
        foreach(var i in LoadedEntities)
            Utility.TryDispose(i);
    }
    
    public void SummonEntity(IEntity entity){
        entity.Init();
        LoadEntity(entity);
    }

    public void LoadEntity(IEntity entity){
        LoadedEntities.Add(entity);
        entity.CurrentChunk = this;
    }

    public void SetTile(Coordinates coordinates, ITile tile){
        this[coordinates] = tile;
        tile.Parent = this;
    }
}