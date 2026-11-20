using AnthillProject.Core.Interfaces;
using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Providing;

namespace AnthillProject.Core.Persistent.Saving;

//This is intended to encode a chunk into a byte array to write into a file and decode it back into a chunk.
public interface IChunkCodec : ICodec<IChunk>, IProvidable;