namespace AnthillProject.Core.Map.Chunks.Generation;
//Does nothing.
public class PlaceholderChunkGenerator : IChunkGenerator{
    public IChunk Generate(IChunk chunk) =>
        chunk;
}