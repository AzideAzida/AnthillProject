using System.Drawing;
using AnthillProject.Core.Map.Tiles;
using AnthillProject.Core.Providing;
namespace AnthillProject.TileBehaviours;

public class Ocean : Tile{
    public override Bitmap GetTexture() =>
        Core.Utility.ArgbBitmap1x1(0, 0, 255);
    public override void Simulate(){
        base.Simulate();
    }
    
    public override void Init(){
        base.Init();
    }

    public Ocean(TileProviderArgs Args):base(Args){}

    public override string GetProviderIdentifier() => ProviderInstance.GetIdentifier();
    internal static IInstanceProvider<Tile> ProviderInstance = new Provider();
    private sealed class Provider : IInstanceProvider<Tile>{
        public Tile ProvideInstance(object Args) =>
            new Ocean((TileProviderArgs)Args);
    }
}
