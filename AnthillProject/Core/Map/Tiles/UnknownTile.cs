using AnthillProject.Core.Providing;

namespace AnthillProject.Core.Map.Tiles;

public class UnknownTile : Tile{
    public override void Simulate(){
        
    }
    
    public override void Init(){
        
    }
    public UnknownTile(TileProviderArgs Args):base(Args){}

    public override string GetProviderIdentifier() => ProviderInstance.GetIdentifier();
    internal static IInstanceProvider<Tile> ProviderInstance = new Provider();
    internal class Provider : IInstanceProvider<Tile>{
        public Tile ProvideInstance(object Args) =>
            new UnknownTile((TileProviderArgs)Args);
    }
}