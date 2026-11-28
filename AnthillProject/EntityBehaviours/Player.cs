using SkiaSharp;
using AnthillProject.Core.Math;
using AnthillProject.Core.Entities;
using AnthillProject.Core;
using AnthillProject.Core.Providing;
using Silk.NET.Input;

namespace AnthillProject.EntityBehaviours;
public sealed class Player : Entity, IDisposable{
    private readonly static List<Player> Players;
    static Player(){
        Players = new List<Player>();
        var client = Program.Instance.Client;
        
        client.OnKeyboardKeyDown += (keyboard, key) => {
            foreach(var i in Players){
                float x = i.CurrentSpeed.x;
                float y = i.CurrentSpeed.y;

                if(key == Key.D) x =  Speed;
                if(key == Key.A) x = -Speed;
                if(key == Key.W) y =  Speed;
                if(key == Key.S) y = -Speed;

                i.CurrentSpeed = new Vector2(x, y);
            }
        };

        client.OnKeyboardKeyUp += (keyboard, key) => {
            foreach(var i in Players){
                float x = i.CurrentSpeed.x;
                float y = i.CurrentSpeed.y;

                if(key == Key.D) x = 0;
                if(key == Key.A) x = 0;
                if(key == Key.W) y = 0;
                if(key == Key.S) y = 0;

                i.CurrentSpeed = new Vector2(x, y);
            }
        };
    }
    public const float Speed = 0.1f;
    private Vector2 CurrentSpeed;

    public Player(EntityProviderArgs i) : base(i){}

    public override Vector2 GetSize() =>
        new Vector2(0.25f);

    public override SKBitmap GetTexture() =>
        Utility.ArgbBitmap1x1(0, 0, 0);

    public override void Simulate(){
        base.Simulate();

        Velocity = CurrentSpeed;
        
        void Load(Coordinates c){
            if(CurrentChunk != null && !CurrentChunk.Parent.IsChunkLoaded(c))
                CurrentChunk.Parent.LoadTempChunkForTicks(
                    new Coordinates(
                        CurrentChunk.SaveCoordinates.x,
                        CurrentChunk.SaveCoordinates.y
                    ) + c, 200);
        }

        //I'm sorry.
        Load(new Coordinates(-1, 1)); Load(new Coordinates(0, 1)); Load(new Coordinates(1, 1));
        Load(new Coordinates(-1, 0)); Load(new Coordinates(0, 0)); Load(new Coordinates(1, 0));
        Load(new Coordinates(-1,-1)); Load(new Coordinates(0,-1)); Load(new Coordinates(1,-1));
    }
    public override void Init(){
        if(!Players.Contains(this))
            Players.Add(this);
    }

    //Disposing the player will make it no longer react to client input.
    public void Dispose(){
        //For some reason this gets called multiple times when closing the window, but I honestly don't give a shit.
        if(Players.Contains(this))
            Players.Remove(this);
    }

    public override string GetProviderIdentifier() => ProviderInstance.GetIdentifier();
    internal static IInstanceProvider<IEntity> ProviderInstance = new Provider();
    private sealed class Provider : IInstanceProvider<IEntity>{
        public IEntity ProvideInstance(object Args) =>
            new UnknownEntity((EntityProviderArgs)Args);
    }
}