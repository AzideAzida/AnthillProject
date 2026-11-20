using AnthillProject.Core.Entities;
using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Math;
using AnthillProject.Core.Persistent.Saving;
using static AnthillProject.Core.Utility;
using AnthillProject.Core.Persistent;
using AnthillProject.Core.Map.Chunks.Generation;
using AnthillProject.EntityBehaviours;

namespace AnthillProject.Core.Server;

public class SingleplayerLocalServer : IServer{
    public const int MillisecondTickInterval = 50;

    public ISaveManager SaveManager;

    private readonly Dictionary<Coordinates, IChunk> LoadedChunks;

    //Loads the chunk for the specified amount of ticks, then it unload and saves it.
    private readonly List<TempChunkEntry> TempChunks;
    private readonly IChunkGenerator ChunkGenerator;
    private sealed class TempChunkEntry{
        public TempChunkEntry(int t, Coordinates c){
            Time = t;
            chunk = c;
        }
        public int Time; 
        public Coordinates chunk;
    }
    
    private readonly int SaveID;
    private bool ContinueExecuting;
    public SingleplayerLocalServer(int Save, ISaveManager saveManager, IChunkGenerator generator){
        SaveID = Save;
        TempChunks = new List<TempChunkEntry>();
        ChunkGenerator = generator;
        
        SaveManager = saveManager;
        LoadedChunks = new Dictionary<Coordinates, IChunk>();
    }

    public async void Init(){
        Console.WriteLine("Server Init() has been called.");

        //Giving the client a head start.
        Thread.Sleep(2000);

        Program.Instance.CurrentEntity = new Player(new EntityProviderArgs()){Velocity = new Vector2(2), Position = new Vector2(1)};

        var CurrentChunk = LoadChunk(new Coordinates(0, 0));

        CurrentChunk.SummonEntity(
            new UnknownEntity(new EntityProviderArgs()){Position = new Vector2(1, 3)}
        );

        //Summoning the player entity.
        CurrentChunk.SummonEntity(
            Program.Instance.CurrentEntity
        );
        
        Console.WriteLine("Starting update loop.");
        
        ContinueExecuting = true;
        
        var ShouldSaveAndExit = false;

        //Make it so when the client closes the server stops executing and the thread stops.
        Program.Instance.Client.OnExit += () => ShouldSaveAndExit = true;

        while(ContinueExecuting){
            long NextTick = GetMilliseconds() + MillisecondTickInterval;
            int GetDelay() => (int)MathF.Abs(GetMilliseconds() - NextTick);

            if(ShouldSaveAndExit){
                SaveAndExit();
                continue;
            }

            for(int i = 0; i < TempChunks.Count; i++){
                try{
                    var entry = TempChunks[i];
                    entry.Time--;
                    if(entry.Time < 0){
                        UnloadAndSaveChunk(entry.chunk);
                        TempChunks.Remove(entry);
                    }
                }
                catch(IndexOutOfRangeException){}
            }

            var loadedChunks = new List<IChunk>();
            foreach(var i in LoadedChunks)
                loadedChunks.Add(i.Value);
                
            for(int i = 0; i < loadedChunks.Count; i++){
                var chunk = loadedChunks[i];
                if(IsChunkLoaded(chunk.SaveCoordinates))
                    chunk.Simulate();
            }
            
            Program.Instance.Client.OnServerTick();


            #if true
                await Task.Delay(GetDelay());
            #else
                var Delay = GetDelay();
                Console.WriteLine(Delay);
                await Task.Delay(Delay);
            #endif
        }
    }

    public void SaveAndExit(){
        foreach(var i in LoadedChunks){
            UnloadAndSaveChunk(i.Key);
        }
        ContinueExecuting = false;
    }

    public IChunk LoadChunk(Coordinates coordinates){
        var chunk = SaveManager.GetChunkFromSave(SaveID, coordinates);
        
        if(chunk == null){
            chunk = ChunkGenerator.Generate(new Chunk());
            chunk.SaveCoordinates = coordinates;
            SaveChunk(chunk);
        }
        chunk.Parent = this;
        
        if(LoadedChunks.TryAdd(coordinates, chunk))
            return chunk;
        
        return GetChunk(coordinates);
    }

    public void UnloadAndSaveChunk(Coordinates coordinates){
        var chunk = LoadedChunks[coordinates];
        SaveChunk(chunk);
        LoadedChunks.Remove(coordinates);
    }

    //If the specified chunk isn't loaded, it'll throw an exception.
    public IChunk GetChunk(Coordinates coordinates){
        try{
            return LoadedChunks[coordinates];
        }
        catch(KeyNotFoundException){
            throw new ArgumentException($"The provided coordinates {coordinates} point to a chunk that isn't loaded! (Did you forget to load it?)");
        }
    }
    public Entity FindEntity(EntityIdentifier identifier) =>
        GetChunk(identifier.ChunkCoordinates).LoadedEntities[identifier.EntityIndex];

    public void SaveChunk(IChunk chunk){
        SaveManager.SaveChunkIntoSave(SaveID, chunk);
    }

    public bool IsChunkLoaded(Coordinates coordinates) =>
        LoadedChunks.ContainsKey(coordinates);

    public void HandleEntityChunkTransition(Entity entity){
        if(entity == null)
            throw new ArgumentException("Cannot handle entity chunk transition for a null entity!");
        if(entity.CurrentChunk == null)
            throw new ArgumentException("Cannot handle an entity chunk transition if the entity's chunk is null!");
            

        entity.CurrentChunk =
            GetChunk(
                entity.CurrentChunk.SaveCoordinates +
                entity.CurrentChunk.GetPossibleExitChunkRelativeCoordinates(entity.CurrentChunk.SaveCoordinates)
            );
        var chunk = entity.CurrentChunk;
        
        var x = entity.Position.x;
        var y = entity.Position.y;

        /**/ if(entity.Position.x >= chunk.GetSizeX()) x = MathF.Abs(chunk.GetSizeX() - entity.Position.x);
        else if(entity.Position.x <  0               ) x =           chunk.GetSizeX() + entity.Position.x ;

        /**/ if(entity.Position.y >= chunk.GetSizeY()) y = MathF.Abs(chunk.GetSizeY() - entity.Position.y);
        else if(entity.Position.y <  0               ) y =           chunk.GetSizeY() + entity.Position.y ;

        entity.Position = new Vector2(x, y);
    }

    public IChunk LoadTempChunkForTicks(Coordinates coordinates, int ticks){
        TempChunks.Add(new TempChunkEntry(ticks, coordinates));
        return LoadChunk(coordinates);
    }
}