using AnthillProject.Core.Providing;

namespace AnthillProject.Core.Map.Tiles;

public class UnknownTile : Tile{
    public override void Simulate(){
        
    }
    
    public override void Init(){
        
    }
    public UnknownTile(TileProviderArgs Args):base(Args){}

    public override string GetProviderIdentifier() => ProviderInstance.GetIdentifier();
    internal static IInstanceProvider<Tile, TileProviderArgs> ProviderInstance = new Provider();
    internal class Provider : IInstanceProvider<Tile, TileProviderArgs>{
        public Tile ProvideInstance(TileProviderArgs Args) =>
            new UnknownTile(Args);
    }
}