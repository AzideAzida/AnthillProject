#pragma warning disable
using AnthillProject.Core.Client;
using AnthillProject.Core.Entities;
using AnthillProject.Core.Map.Tiles;
using AnthillProject.Core.Server;
using AnthillProject.Core.Providing;

using AnthillProject.TileBehaviours;
using AnthillProject.EntityBehaviours;

namespace AnthillProject.Core;
public sealed class Program{
    public static Program Instance{get; private set;}

    public IServer Server;
    public IClient Client;
    public IInstanceProviderManager InstanceProviderManager;

    public Entity CurrentEntity;

    [STAThread]
    private static void Main() => new Program();
    public Program(){
        if(Instance == null)
            Instance = this;
        else
            throw new InvalidOperationException("Cannot instantiate Program because it's instance isn't null!");

        InstanceProviderManager = new DefaultInstanceProviderManager();

        Client = new DefaultLocalClient();

        InstanceProviderManager.RegisterInstanceProvider(UnknownTile    .ProviderInstance);
        InstanceProviderManager.RegisterInstanceProvider(Ocean          .ProviderInstance);
        InstanceProviderManager.RegisterInstanceProvider(Grassland      .ProviderInstance);
        InstanceProviderManager.RegisterInstanceProvider(UnknownEntity  .ProviderInstance);
        InstanceProviderManager.RegisterInstanceProvider(Player         .ProviderInstance);

        //Instantiating the server.
        //Once I get a proper client with a main menu and stuff like that, the client will take of instantiating the server and creating it's thread.
        Server = new SingleplayerLocalServer(0);
        
        //Server thread.
        new Thread(new ThreadStart(Server.Init)).Start();
        
        Client.Init();
    }
}
