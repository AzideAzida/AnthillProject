using AnthillProject.Core.Entities;
using AnthillProject.Core.Map.Tiles;
using AnthillProject.Core.Math;
using AnthillProject.Core.Server;

namespace AnthillProject.Core.Map.Chunks;

public sealed class InvalidChunk : IChunk{
    public InvalidChunk(string reason = "No reason was provided."){
        Reason = reason;
    }
    public readonly string Reason;
    private Exception ResultantException => new NotSupportedException(
        $"Cannot perform operation on invalid chunk! Reason: {Reason}");
    public IList<IEntity> LoadedEntities{
        get => throw ResultantException;
        set => throw ResultantException;
    }
    public Coordinates SaveCoordinates{
        get => throw ResultantException;
        set => throw ResultantException;
    }
    public IServer Parent{
        get => throw ResultantException;
        set => throw ResultantException;
    }

    public Coordinates GetCoordinates(ITile tile){
        throw ResultantException;
    }

    public int GetSizeX(){
        throw ResultantException;
    }

    public int GetSizeY(){
        throw ResultantException;
    }

    public ITile GetTile(Coordinates coordinates){
        throw ResultantException;
    }

    public void LoadEntity(IEntity entity){
        throw ResultantException;
    }

    public void SetTile(Coordinates coordinates, ITile tile){
        throw ResultantException;
    }

    public void Simulate(){
        throw ResultantException;
    }

    public void SummonEntity(IEntity entity){
        throw ResultantException;
    }
}