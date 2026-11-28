using SkiaSharp;
using AnthillProject.Core.Map.Tiles;
using AnthillProject.Core.Providing;
using AnthillProject.Core.Utilities;
namespace AnthillProject.TileBehaviours;

public class Ocean : Tile{
    private readonly static SKBitmap Texture = ReflectionUtils.RetrieveTexture($"Tiles.{nameof(Ocean)}.png");
    public override SKBitmap GetTexture() => Texture;
    public override void Simulate(){
        base.Simulate();
    }
    
    public override void Init(){
        base.Init();
    }

    public Ocean(TileProviderArgs Args):base(Args){}

    public override string GetProviderIdentifier() => ProviderInstance.GetIdentifier();
    internal static IInstanceProvider<ITile> ProviderInstance = new Provider();
    private sealed class Provider : IInstanceProvider<ITile>{
        public ITile ProvideInstance(object Args) =>
            new Ocean((TileProviderArgs)Args);
    }
}
