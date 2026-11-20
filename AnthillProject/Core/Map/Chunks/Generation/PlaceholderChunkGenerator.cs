using AnthillProject.Core.Providing;

namespace AnthillProject.Core.Map.Chunks.Generation;
//Does nothing.
public class PlaceholderChunkGenerator : IChunkGenerator{
    public IChunk Generate(IChunk chunk) =>
        chunk;
    
    public byte[] ArbitraryData{get => []; set{}}
    public string GetProviderIdentifier() => ProviderInstance.GetIdentifier();
    internal static IInstanceProvider<IChunkGenerator> ProviderInstance = new Provider();
    private sealed class Provider : IInstanceProvider<IChunkGenerator>{
        public IChunkGenerator ProvideInstance(object Args) =>
            new PlaceholderChunkGenerator();
    }
}