using SkiaSharp;
using AnthillProject.Core.Math;
using AnthillProject.Core.Entities;
using AnthillProject.Core;
using AnthillProject.Core.Providing;
using Silk.NET.Input;

namespace AnthillProject.EntityBehaviours;
public class Player : Entity{
    public const float Speed = 0.1f;

    public Player(EntityProviderArgs i) : base(i){}

    public override Vector2 GetSize() =>
        new Vector2(0.25f);

    public override SKBitmap GetTexture() =>
        Utility.ArgbBitmap1x1(0, 0, 0);

    public override void Simulate(){
        base.Simulate();

        //Velocity = new Vector2(0.1f);
        //Console.WriteLine();
        
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
    public override void Init(){}

    public override string GetProviderIdentifier() => ProviderInstance.GetIdentifier();
    internal static IInstanceProvider<Entity> ProviderInstance = new Provider();
    private sealed class Provider : IInstanceProvider<Entity>{
        public Entity ProvideInstance(object Args) =>
            new UnknownEntity((EntityProviderArgs)Args);
    }
}