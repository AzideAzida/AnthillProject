#pragma warning disable
using AnthillProject.Core.Client;
using AnthillProject.Core.Entities;
using AnthillProject.Core.Map.Tiles;
using AnthillProject.Core.Server;
using AnthillProject.Core.Providing;

using AnthillProject.TileBehaviours;
using AnthillProject.EntityBehaviours;
using AnthillProject.Core.Persistent.Saving;
using AnthillProject.ChunkGenerators;
using AnthillProject.Core.Map.Chunks.Generation;
using AnthillProject.Core.Persistent;

namespace AnthillProject.Core;
public sealed class Program{
    public static Program Instance{get; private set;}

    public IServer Server;
    public IClient Client;
    public IInstanceProviderManager InstanceProviderManager;
    public IPersistentDataManager PersistentDataManager;
    public ISaveManager SaveManager;

    public Entity CurrentEntity;

    [STAThread]
    private static void Main() => new Program();
    public Program(){
        if(Instance == null)
            Instance = this;
        else
            throw new InvalidOperationException("Cannot instantiate Program because it's instance isn't null!");
        
        //Deletes all persistent data. This is only temporary until I work all the kinks out of the persistence system.
        Directory.Delete("./PersistentData", true);

        InstanceProviderManager = new DefaultInstanceProviderManager();
        Client = new DefaultLocalClient();

        InstanceProviderManager.RegisterInstanceProvider(UnknownTile                .ProviderInstance);
        InstanceProviderManager.RegisterInstanceProvider(Ocean                      .ProviderInstance);
        InstanceProviderManager.RegisterInstanceProvider(Grassland                  .ProviderInstance);
        InstanceProviderManager.RegisterInstanceProvider(UnknownEntity              .ProviderInstance);
        InstanceProviderManager.RegisterInstanceProvider(Player                     .ProviderInstance);
        InstanceProviderManager.RegisterInstanceProvider(DefaultChunkCodecV1        .ProviderInstance);
        InstanceProviderManager.RegisterInstanceProvider(TestChunkGenerator         .ProviderInstance);
        InstanceProviderManager.RegisterInstanceProvider(PlaceholderChunkGenerator  .ProviderInstance);

        PersistentDataManager = new DefaultPersistentDataManager();
        SaveManager = new DefaultDiskSaveManager(PersistentDataManager);

        var ChunkGenerator = new TestChunkGenerator();
        if(!SaveManager.SaveExists(0))
            SaveManager.CreateSave(0, ChunkGenerator);

        //Once I get a proper client with a main menu and stuff like that, the client will take of getting the server and creating it's thread.
        Server = SaveManager.GetSave(0);
        
        //Server thread.
        new Thread(new ThreadStart(Server.Init)).Start();
        
        Client.Init();
    }
}
