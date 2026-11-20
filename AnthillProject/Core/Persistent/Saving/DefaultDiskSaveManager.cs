using System.Text;
using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Map.Chunks.Generation;
using AnthillProject.Core.Math;
using AnthillProject.Core.Providing;
using AnthillProject.Core.Server;

namespace AnthillProject.Core.Persistent.Saving;

public class DefaultDiskSaveManager : ISaveManager{
    protected virtual string[] CoreDirs => [
        GetSavesDir()
    ];

    protected virtual string GetSavesDir() => $"Saves/";
    protected virtual string GetSaveDir(int save) => $"{GetSavesDir()}/{save}";
    protected virtual string GetChunkDir(int save, Coordinates cords) =>
        $"{GetSaveDir(save)}/Chunks/{
            Convert.ToHexString(Utility.GetBytesBE(cords.x))
        }{  Convert.ToHexString(Utility.GetBytesBE(cords.y))}";
    
    public readonly IPersistentDataManager PersistentDataManager;
    public readonly IChunkCodec ChunkCodec;
    public DefaultDiskSaveManager(IPersistentDataManager p):this(p, new DefaultChunkCodecV1()){}
    public DefaultDiskSaveManager(IPersistentDataManager persistentDataManager, IChunkCodec codec){
        PersistentDataManager = persistentDataManager;
        ChunkCodec = codec;
        ListAllSaves = new List<int>();
        
        foreach(var i in CoreDirs)
            if(!PersistentDataManager.DirectoryExists(i))
                PersistentDataManager.CreateDirectory(i);
    

        foreach(var i in PersistentDataManager.EnumerateDirectories("Saves")){
            ListAllSaves.Add(Convert.ToInt32(i.Replace("./PersistentData\\Saves\\", "")));
        }
    }
    private readonly List<int> ListAllSaves;
    public int[] AllSaves => ListAllSaves.ToArray();

    public void CreateSave(int save, IChunkGenerator generator){
        if(ListAllSaves.Contains(save))
            throw new ArgumentException($"Cannot create save {save} because such save already exists.");
        PersistentDataManager.CreateDirectory(GetSaveDir(save));
        PersistentDataManager.CreateDirectory(GetChunkDir(save, Coordinates.Zero));
        
        List<byte> dat = [
            //The coordinates of the entry chunk.
            0, 0, 0, 0, //X
            0, 0, 0, 0  //Y
        ];
        SaveChunkIntoSave(save, generator.Generate(new Chunk()));

        dat.AddRange(Encoding.ASCII.GetBytes(generator.GetProviderIdentifier()));

        PersistentDataManager.WriteToFile(GetSaveDir(save)+"/.dat", dat.ToArray());
    }

    public IChunk? GetChunkFromSave(int save, Coordinates coordinates){
        if(!ChunkExistsInSave(save, coordinates))
            return null;
        
        var Dir = GetChunkDir(save, coordinates);

        var chunk =
            Program.Instance.InstanceProviderManager.ProvideInstance<IChunkCodec>(
                PersistentDataManager.ReadFileAsString($"{Dir}/c.txt"), new DummyInstanceProviderArgs()
            ).Decode(
                PersistentDataManager.ReadFileAsBytes($"{Dir}/.bin")
            );

        chunk.SaveCoordinates = coordinates;

        return chunk;
    }

    public void SaveChunkIntoSave(int save, IChunk chunk){
        var Dir = GetChunkDir(save, chunk.SaveCoordinates);
        if(!PersistentDataManager.DirectoryExists(Dir))
            PersistentDataManager.CreateDirectory(Dir);
        PersistentDataManager.WriteToFile(
            $"{Dir}/.bin",
            ChunkCodec.Encode(chunk)
        );
        PersistentDataManager.WriteToFile(
            $"{Dir}/c.txt",
            ChunkCodec.GetProviderIdentifier()
        );
    }

    public bool SaveExists(int save) =>
        PersistentDataManager.DirectoryExists(GetSaveDir(save));
    
    public bool ChunkExistsInSave(int save, Coordinates coordinates){
        if(!SaveExists(save))
            return false;
        return PersistentDataManager.DirectoryExists(GetChunkDir(save, coordinates));
    }

    public IServer? GetSave(int save){
        var dat = PersistentDataManager.ReadFileAsBytes($"{GetSaveDir(save)}/.dat");
        var coordinates = new Coordinates(
            Utility.IntFromBytesBE([dat[0], dat[1], dat[2], dat[3]]),
            Utility.IntFromBytesBE([dat[4], dat[5], dat[6], dat[7]])
        );
        List<byte> generator = [];
        for(int i = 8; i < dat.Length; i++)
            generator.Add(dat[i]);
        

        var server = new SingleplayerLocalServer(save, this,
            Program.Instance.InstanceProviderManager.ProvideInstance<IChunkGenerator>(
                Encoding.ASCII.GetString(generator.ToArray()),
                new DummyInstanceProviderArgs()
            )
        );

        server.LoadChunk(coordinates);

        return server;
    }
}