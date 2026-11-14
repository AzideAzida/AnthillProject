using System.Drawing;
using AnthillProject.Core.Map.Tiles;
using AnthillProject.Core.Providing;
namespace AnthillProject.TileBehaviours;

public class Grassland : Tile{
    public override Image GetTexture() =>
        Core.Utility.ArgbBitmap1x1(0, 255, 0);
    public override void Simulate(){
        
    }
    
    public override void Init(){
        
    }

    public Grassland(TileProviderArgs Args):base(Args){}

    public override string GetProviderIdentifier() => ProviderInstance.GetIdentifier();
    internal static IInstanceProvider<Tile, TileProviderArgs> ProviderInstance = new Provider();
    internal class Provider : IInstanceProvider<Tile, TileProviderArgs>{
        public Tile ProvideInstance(TileProviderArgs Args) =>
            new Grassland(Args);
    }
}