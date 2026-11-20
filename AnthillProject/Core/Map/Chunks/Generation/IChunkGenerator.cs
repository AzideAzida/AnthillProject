using AnthillProject.Core.Providing;

namespace AnthillProject.Core.Map.Chunks.Generation;
//When creating a chunk, it should pass through this. This should make any required modifications, and then return the modified chunk.
public interface IChunkGenerator : IProvidable{
    IChunk Generate(IChunk chunk);
}