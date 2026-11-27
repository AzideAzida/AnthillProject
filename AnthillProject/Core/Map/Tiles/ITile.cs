using AnthillProject.Core.Map.Chunks;
using AnthillProject.Core.Interfaces;
using AnthillProject.Core.Providing;

namespace AnthillProject.Core.Map.Tiles;
public interface ITile : IProvidable, ISimulable, IInitializable, ITextured, IParented<IChunk>;