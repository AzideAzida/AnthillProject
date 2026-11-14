namespace AnthillProject.Core.Interfaces;
public interface ICodec<T>{
    byte[] Encode(T chunk);
    T Decode(byte[] bytes);
}
