using SkiaSharp;
using AnthillProject.Core.Math;
using AnthillProject.Core.Providing;

namespace AnthillProject.Core.Entities;
public class UnknownEntity : Entity{
    public UnknownEntity(EntityProviderArgs i) : base(i){}

    public override Vector2 GetSize() =>
        new Vector2(0.5f, 0.5f);

    public override SKBitmap GetTexture() =>
        Core.Utility.ArgbBitmap1x1(255, 0, 255);

    public override void Simulate(){
        base.Simulate();
    }
    public override void Init(){
        base.Init();
    }

    public override string GetProviderIdentifier() => ProviderInstance.GetIdentifier();
    internal static IInstanceProvider<Entity> ProviderInstance = new Provider();
    private sealed class Provider : IInstanceProvider<Entity>{
        public Entity ProvideInstance(object Args) =>
            new UnknownEntity((EntityProviderArgs)Args);
    }
}