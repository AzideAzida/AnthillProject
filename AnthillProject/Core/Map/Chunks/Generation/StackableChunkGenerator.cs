using System.Collections;

namespace AnthillProject.Core.Map.Chunks.Generation;
//This holds multiple IChunkGenerators and executes them in series.
//You can put other StackableChunkGenerators in it if your heart so desires.
public class ChunkGeneratorStack : IChunkGenerator, IEnumerable<IChunkGenerator>{
    #region Interface implementations
        public IChunk Generate(IChunk chunk){
            foreach(var i in Generators)
                chunk = i.Generate(chunk);
            return chunk;
        }

        public IEnumerator<IChunkGenerator> GetEnumerator() => Generators.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion

    public List<IChunkGenerator> Generators;
    public ChunkGeneratorStack(List<IChunkGenerator> generators){
        if(generators == null)
            throw new ArgumentNullException("Cannot create a StackableChunkGenerator with a null list! (Did you mean to use the parameterless constructor?)");
        
        Generators = generators;
    }
    public ChunkGeneratorStack() =>
        Generators = new List<IChunkGenerator>();
    
    public void Add(IChunkGenerator generator) =>
        Generators.Add(generator);
    public bool Remove(IChunkGenerator generator) =>
        Generators.Remove(generator);
}