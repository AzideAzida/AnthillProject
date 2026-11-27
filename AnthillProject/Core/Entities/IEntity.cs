using AnthillProject.Core.Math;
using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Interfaces;
using AnthillProject.Core.Providing;

namespace AnthillProject.Core.Entities;
public interface IEntity : IProvidable, ISimulable, IInitializable, ITextured{
    //1 unit in the size is 1 tile.
    Vector2 GetSize();

    //1 unit in the position is 1 tile.
    Vector2 Position{get; set;}

    //Velocity should be tiles per tick.
    Vector2 Velocity{get; set;}

    //You should make sure that the entity is loaded in the current chunk, whatever it may be.
    IChunk? CurrentChunk{get; set;}
}