using AnthillProject.Core.Entities;
using AnthillProject.Core.Interfaces;
using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Math;

namespace AnthillProject.Core.Server;

//Init() will be called in a new thread; there is no need for you to make a new thread yourself.
//The server should create a loop where all simulation happens in Init(), with Task.Delay or other stuff so it doesn't simulate 1000 times per second.
//The server should also simulate all loaded chunks.
public interface IServer : IInitializable{
    //Should be invoked every tick.
    event Action OnServerTickFinish;

    //Should save the provided chunk to it's save coordinates.
    //It is recommended to use an ISaveManager.
    void SaveChunk(IChunk chunk);

    //Should save the provided chunk to it's save coordinates asynchronously.
    //It is recommended to use an ISaveManager.
    //OnFinish should be called when it is finished.
    //If OnFinish is null, do nothing.
    void SaveChunkAsync(IChunk chunk, Action? OnFinish = null);

    //Should save all loaded chunks to disk or to whatever saving system you have set up.
    //It is recommended to use an ISaveManager.
    void SaveAndExit();

    //Should save and exit asynchronously. OnFinish should be called when it is finished.
    //If OnFinish is null, do nothing.
    void SaveAndExitAsync(Action? OnFinish = null);

    //This should pull the chunk directly from your saving system and load it into a cache of sorts.
    //It should then return that chunk.
    //You should always remember to unload it, unless you need it to stay loaded for a niche use.
    //If the chunk is already loaded, it should return that chunk and do nothing else.
    //If your saving system does not have a chunk with these coordinates, you should generate it.
    IChunk LoadChunk(Coordinates coordinates);

    //Should load the chunk asynchronously. OnFinish should be called when it is finished and be provided with the loaded chunk.
    //If OnFinish is null, do nothing.
    void LoadChunkAsync(Coordinates coordinates, Action<IChunk>? OnFinish = null);

    //Same as LoadChunk, but should automatically unload it after the specified amount of ticks have passed after it's been loaded.
    //If the chunk is already loaded, it should return that chunk and unload it after the time has passed.
    IChunk LoadTempChunkForTicks(Coordinates coordinates, int ticks);

    //Should load the chunk asynchronously for the specified ticks. OnFinish should be called when it is finished and be provided with the loaded chunk.
    //If OnFinish is null, do nothing.
    void LoadTempChunkForTicksAsync(Coordinates coordinates, int ticks, Action<IChunk>? OnFinish = null);

    //This should remove the chunk that the coordinates point to from the cache that LoadChunk should create and save it with your saving system.
    void UnloadAndSaveChunk(Coordinates coordinates);

    //Should unload and save the chunk asynchronously. OnFinish should be called when it is finished.
    //If OnFinish is null, do nothing.
    void UnloadAndSaveChunkAsync(Coordinates coordinates, Action? OnFinish = null);

    bool IsChunkLoaded(Coordinates coordinates);

    //This is similar to an ISaveManager's GetChunkFromSave.
    //It should not however pull it directly from an ISaveManager, as it may pull it from disk which will be   s l o w.
    //It should instead pull it from the cache that LoadChunk should create.
    //If the chunk that the coordinates point to isn't loaded, you can load and return it, pull it directly from your saving system or throw an exception.
    IChunk GetChunk(Coordinates coordinates);

    IEntity FindEntity(EntityIdentifier identifier);

    //This should take care of moving an entity from one chunk to another.
    //This also includes figuring out it's new position and chunk coordinate.
    void HandleEntityChunkTransition(IEntity entity);
}