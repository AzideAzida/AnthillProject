using AnthillProject.Core.Math;

namespace AnthillProject.Core.Tiling;

public interface ITiled<T> : IEnumerable<T>{
    T this[Coordinates coord]{get; set;}
}