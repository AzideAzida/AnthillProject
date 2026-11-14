using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Math;
using AnthillProject.Core.Server;

namespace AnthillProject.Core.Persistent.Saving;

public interface ISaveManager{
    void CreateSave(int save);
    bool SaveExists(int save);
    int[] AllSaves{get;}

    IServer GetSave(int save);

    IChunk GetChunkFromSave(int save, Coordinates coordinates);
    bool ChunkExistsInSave(int save, Coordinates coordinates);
    void SaveChunkIntoSave(int save, IChunk chunk);
}