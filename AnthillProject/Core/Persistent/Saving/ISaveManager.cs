using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Map.Chunks.Generation;
using AnthillProject.Core.Math;
using AnthillProject.Core.Server;

namespace AnthillProject.Core.Persistent.Saving;

public interface ISaveManager{
    void CreateSave(int save, IChunkGenerator generator);
    bool SaveExists(int save);
    int[] AllSaves{get;}

    //Should return null if no save exists.
    IServer? GetSave(int save);

    //Should return null if no save or chunk exists.
    IChunk? GetChunkFromSave(int save, Coordinates coordinates);
    bool ChunkExistsInSave(int save, Coordinates coordinates);
    void SaveChunkIntoSave(int save, IChunk chunk);
}