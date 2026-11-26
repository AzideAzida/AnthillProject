using SkiaSharp;
using AnthillProject.Core.Map.Tiles;
using AnthillProject.Core.Providing;
namespace AnthillProject.TileBehaviours;

public class Grassland : Tile{
    public override SKBitmap GetTexture() =>
        Core.Utility.ArgbBitmap1x1(0, 255, 0);
    public override void Simulate(){
        
    }
    
    public override void Init(){
        
    }

    public Grassland(TileProviderArgs Args):base(Args){}

    public override string GetProviderIdentifier() => ProviderInstance.GetIdentifier();
    internal static IInstanceProvider<Tile> ProviderInstance = new Provider();
    private sealed class Provider : IInstanceProvider<Tile>{
        public Tile ProvideInstance(object Args) =>
            new Grassland((TileProviderArgs)Args);
    }
}