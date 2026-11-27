using SkiaSharp;
using AnthillProject.Core.Map.Tiles;
using AnthillProject.Core.Providing;
using AnthillProject.Core;
namespace AnthillProject.TileBehaviours;

public class Grassland : Tile{
    private readonly static SKBitmap Texture = Utility.RetrieveTexture($"Tiles.{nameof(Grassland)}.png");
    public override SKBitmap GetTexture() => Texture;
    public override void Simulate(){
        base.Simulate();
    }
    
    public override void Init(){
        base.Init();
    }

    public Grassland(TileProviderArgs Args):base(Args){}

    public override string GetProviderIdentifier() => ProviderInstance.GetIdentifier();
    internal static IInstanceProvider<ITile> ProviderInstance = new Provider();
    private sealed class Provider : IInstanceProvider<ITile>{
        public ITile ProvideInstance(object Args) =>
            new Grassland((TileProviderArgs)Args);
    }
}