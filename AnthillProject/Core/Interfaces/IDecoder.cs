namespace AnthillProject.Core.Interfaces;
public interface IDecoder<T>{
    T Decode(byte[] bytes);
}
