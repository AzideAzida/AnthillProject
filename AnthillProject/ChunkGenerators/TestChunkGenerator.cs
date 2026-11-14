using AnthillProject.Core;
using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Map.Chunks.Generation;
using AnthillProject.Core.Map.Tiles;
using AnthillProject.Core.Math;
using AnthillProject.TileBehaviours;

namespace AnthillProject.ChunkGenerators;

public class TestChunkGenerator : IChunkGenerator{
    public readonly int Seed;
    public IChunk Generate(IChunk chunk){
        for(byte x = 0; x < chunk.GetSizeX(); x++)
            for(byte y = 0; y < chunk.GetSizeY(); y++)
                if(
                    new Random(
                        x + (
                            y * chunk.GetSizeY()
                        ) + Seed +

                        (((x * chunk.SaveCoordinates.x) + (y * chunk.SaveCoordinates.y)) * 1024)
                    ).Next(0, 3) == 0
                )
                    chunk.SetTile(new Coordinates(x, y), Program.Instance.InstanceProviderManager.ProvideInstance<Tile, TileProviderArgs>(Grassland.ProviderInstance.GetIdentifier(), new TileProviderArgs()));
                else
                    chunk.SetTile(new Coordinates(x, y), Program.Instance.InstanceProviderManager.ProvideInstance<Tile, TileProviderArgs>(Ocean    .ProviderInstance.GetIdentifier(), new TileProviderArgs()));
                
        return chunk;
    }
    public TestChunkGenerator(int seed = 0) =>
        Seed = seed;
}