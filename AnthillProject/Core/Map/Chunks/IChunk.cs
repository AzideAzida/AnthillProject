using AnthillProject.Core.Entities;
using AnthillProject.Core.Math;
using AnthillProject.Core.Map.Tiles;
using AnthillProject.Core.Server;
using AnthillProject.Core.Interfaces;

namespace AnthillProject.Core.Map.Chunks;

public interface IChunk : ISimulable, IParented<IServer>{
    int GetSizeX();
    int GetSizeY();
    
    //First tile is is 0, 0.
    Tile GetTile(Coordinates coordinates);
    void SetTile(Coordinates coordinates, Tile tile);
    Coordinates GetCoordinates(Tile tile);

    //Should initialize and load the entity.
    //This should be used when spawing an entity, and not when (for example) moving one from one chunk to another.
    void SummonEntity(IEntity entity);

    //Places the entity in the chunk. This should not initialize it.
    void LoadEntity(IEntity entity);
    IList<IEntity> LoadedEntities{get; set;}

    //When an ISaveManager saves a provided chunk, it will get saved to these coordinates.
    //Likewise, when an ISaveManager loads an existing chunk from coordinates, this will be set to those coordinates.
    Coordinates SaveCoordinates{get; set;}

    //Should check wether the provided values are out of bounds depending on GetSize methods.
    //If they are, return the coordinates of the chunk it would then move to relative to this chunk.
    //If they aren't, return (0, 0).
    Coordinates GetPossibleExitChunkRelativeCoordinates(Coordinates coordinates) =>
        //Also, nifty little implicit body in interface! Didn't know you could do this.
        new Coordinates(
            (int)MathF.Floor(coordinates.x / GetSizeX()),
            (int)MathF.Floor(coordinates.y / GetSizeY())
        );
}