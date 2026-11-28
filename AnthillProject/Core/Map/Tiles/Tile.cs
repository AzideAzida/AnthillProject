#pragma warning disable CS8618
using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Utilities;
using SkiaSharp;

namespace AnthillProject.Core.Map.Tiles;
public abstract class Tile : ITile{
    public virtual void Init(){}
    public virtual void Simulate(){}
    public virtual SKBitmap GetTexture() =>
        GraphicUtils.ArgbBitmap1x1(255, 0, 255);

    public abstract string GetProviderIdentifier();

    public Tile(TileProviderArgs Args){}

    public virtual byte[] GetArbitraryData() => [];

    public IChunk Parent{get; set;}
}