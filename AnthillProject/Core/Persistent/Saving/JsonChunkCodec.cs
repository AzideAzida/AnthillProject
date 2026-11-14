using System.Text.Json;
using AnthillProject.Core.Map.Chunks;

namespace AnthillProject.Core.Persistent.Saving;

public class JsonChunkCodec : IChunkCodec{
    public IChunk Decode(byte[] bytes){
        throw new NotImplementedException();
    }

    public byte[] Encode(IChunk chunk){
        //using var Json = new JsonDocument();

        throw new NotImplementedException();
    }
}