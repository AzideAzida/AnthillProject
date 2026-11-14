using AnthillProject.Core.Math;

namespace AnthillProject.Core.Entities;
//This provides a way to reference any entity anywhere in a save.
//If the entity moves chunk, this will no longer work.
//This is used mostly to search for a specific entity in a save that isn't being simulated.
public readonly struct EntityIdentifier{
    public readonly Coordinates ChunkCoordinates;
    public readonly int EntityIndex;
}