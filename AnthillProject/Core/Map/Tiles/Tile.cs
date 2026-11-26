#pragma warning disable CS8618
using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Interfaces;
using AnthillProject.Core.Providing;
using SkiaSharp;

namespace AnthillProject.Core.Map.Tiles;
public abstract class Tile : IProvidable, ISimulable, IInitializable, ITextured, IParented<IChunk>{
    public virtual void Init(){}
    public virtual void Simulate(){}
    public virtual SKBitmap GetTexture() =>
        Utility.ArgbBitmap1x1(255, 0, 255);

    public abstract string GetProviderIdentifier();

    public Tile(TileProviderArgs Args){
        ArbitraryData = Args.Arbitrary;
    }

    public byte[] ArbitraryData{get; set;}

    public IChunk Parent{get; set;}
}