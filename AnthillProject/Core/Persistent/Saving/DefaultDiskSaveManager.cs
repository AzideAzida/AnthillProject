using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Math;
using AnthillProject.Core.Server;

namespace AnthillProject.Core.Persistent.Saving;

public class DefaultDiskSaveManager : ISaveManager{
    protected string GetSaveDir(int save) => $"Saves/{save}";
    public readonly IPersistentDataManager PersistentDataManager;
    public DefaultDiskSaveManager(IPersistentDataManager persistentDataManager){
        PersistentDataManager = persistentDataManager;
        ListAllSaves = new List<int>();
    }
    private readonly List<int> ListAllSaves;
    public int[] AllSaves => ListAllSaves.ToArray();

    public void CreateSave(int save){
        if(ListAllSaves.Contains(save))
            throw new ArgumentException($"Cannot create save {save} because such save already exists.");
        PersistentDataManager.CreateDirectory(GetSaveDir(save));
    }

    public IChunk GetChunkFromSave(int save, Coordinates coordinates){
        throw new NotImplementedException();
    }

    public void SaveChunkIntoSave(int save, IChunk chunk){
        throw new NotImplementedException();
    }

    public bool SaveExists(int save){
        throw new NotImplementedException();
    }

    public bool ChunkExistsInSave(int save, Coordinates coordinates){
        throw new NotImplementedException();
    }

    public IServer GetSave(int save){
        throw new NotImplementedException();
    }
}