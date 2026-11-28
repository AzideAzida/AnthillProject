//"This code is self documenting" and other hilarious jokes I can tell myself.

using AnthillProject.Core.Entities;
using AnthillProject.Core.Math;
using AnthillProject.Core.Map.Tiles;
using AnthillProject.Core.Server;
using SkiaSharp;
using AnthillProject.TileBehaviours;
using AnthillProject.Core.Utilities;

namespace AnthillProject.Core.Map.Chunks;

public readonly struct ChunkProfile : IChunk{
    private class DummyEntity : Entity{
        private readonly IEntity Source;
        public override Vector2 GetSize() => 
            Source.GetSize();

        public override SKBitmap GetTexture() =>
            Source.GetTexture();
        public override string GetProviderIdentifier() =>
            Source.GetProviderIdentifier();
        
        public DummyEntity(IEntity src, Vector2 position){
            Source = src;
            Position = position;
        }
    }
    public int GetSizeX() => SizeX;
    private readonly int SizeX;
    public int GetSizeY() => SizeY;
    private readonly int SizeY;
    public ITile GetTile(Coordinates coordinates) =>
        Tiles[coordinates.x, coordinates.y];
    public void SetTile(Coordinates coordinates, ITile tile) =>
        Tiles[coordinates.x, coordinates.y] = tile;
    
    public void OnUnload(){}
    
    private readonly ITile[,] Tiles;
    public ChunkProfile(IEntity TargetEntity, byte lenght, byte height){
        //This is some bullshit code right here.
        
        SizeX = lenght;
        SizeY = height;
        
        loadedEntities = new List<IEntity>();
        Tiles = new ITile[GetSizeX(), GetSizeY()];
        if(
                TargetEntity              == null
            ||  TargetEntity.CurrentChunk == null
        ) return;

        var xHalf = GetSizeX() / 2;
        var yHalf = GetSizeY() / 2;

        for(int x = 0; x < GetSizeX(); x++)
            for(int y = 0; y < GetSizeY(); y++){
                try{
                    //This gets floored because some weird shit happens if you don't. Took me a week to figure that out...
                    var TileCoordinateX = (int)MathF.Floor(TargetEntity.Position.x - xHalf + x);
                    var TileCoordinateY = (int)MathF.Floor(TargetEntity.Position.y - yHalf + y);

                    var EntityChunk = TargetEntity.CurrentChunk;
                    var CurrentServer = EntityChunk.Parent;
                    var ChunkCoordinate = EntityChunk.SaveCoordinates +
                        EntityChunk.GetPossibleExitChunkRelativeCoordinates(new Coordinates(TileCoordinateX, TileCoordinateY));
                    var CurrentChunk = CurrentServer.GetChunk(ChunkCoordinate);

                    TileCoordinateX = (byte)TileCoordinateX % CurrentChunk.GetSizeX();
                    TileCoordinateY = (byte)TileCoordinateY % CurrentChunk.GetSizeY();
                    //Console.WriteLine($"{TileCoordinateX} {TileCoordinateY}");

                    Tiles[x, y] = CurrentChunk.GetTile(new Coordinates(TileCoordinateX, TileCoordinateY));

                    //Getting the entities. Kinda scuffed, but will do for now.
                    foreach(var i in CurrentChunk.LoadedEntities){
                        if(
                            MathF.Floor(i.Position.x) == TileCoordinateX &&
                            MathF.Floor(i.Position.y) == TileCoordinateY
                        )

                        loadedEntities.Add(
                            new DummyEntity(i,
                                new Vector2(
                                    x + MathUtils.GetDecimals(i.Position.x),
                                    y + MathUtils.GetDecimals(i.Position.y)
                                )
                            )
                        );
                    }
                }
                catch{
                    Tiles[x, y] = new Grassland(new TileProviderArgs());
                }
            }
        
        //Jesus fucking christ, that was fucking horrific. Let's NEVER do that again, PLEASE!
    }

    private readonly List<IEntity> loadedEntities;
    public readonly IList<IEntity> LoadedEntities{
        get => new List<IEntity>(loadedEntities);

    //Unsopported stuff
        set => throw new NotSupportedException("Cannot set a ChunkProfile's LoadedEntities property.");
    }
    public void Simulate() =>
        throw new NotSupportedException("ChunkProfiles cannot be simulated!");
    public void SummonEntity(IEntity entity) =>
        throw new NotSupportedException("Cannot summon entities in a ChunkProfile!");
    public void LoadEntity(IEntity entity) =>
        throw new NotSupportedException("Cannot load entities in an existing ChunkProfile!");
    
    public IServer Parent{
        readonly get => throw new NotSupportedException("ChunkProfiles cannot have parents!");
        set => throw new NotSupportedException("ChunkProfiles cannot have parents!");
    }
    public Coordinates SaveCoordinates{
        get => throw new NotSupportedException("Because ChunkProfiles can't (or rather shouldn't) be saved, they cannot have SaveCoordinates.");
        set => throw new NotSupportedException("Because ChunkProfiles can't (or rather shouldn't) be saved, they cannot have SaveCoordinates.");
    }
    public Coordinates GetPossibleExitChunkRelativeCoordinates(int x, int y) =>
        throw new NotSupportedException("Since ChunkProfiles cannot have parents, they cannot have neighboor chunks.");
    public Coordinates GetCoordinates(ITile tile) =>
        throw new NotSupportedException("ChunkProfiles cannot provide a tile's coordinates.");
}
