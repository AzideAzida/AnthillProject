using AnthillProject.Core.Math;
using AnthillProject.Core.Providing;

namespace AnthillProject.Core.Map.Tiles;

public readonly struct TileProviderArgs : IInstanceProviderArgs<Tile>{
    public readonly byte[] Arbitrary;
    public TileProviderArgs(): this([]){}
    public TileProviderArgs(byte[] arbitrary){
        Arbitrary = arbitrary;
    }
    public byte[] GetArbitraryData() => Arbitrary;
}