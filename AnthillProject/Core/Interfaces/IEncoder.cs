namespace AnthillProject.Core.Interfaces;
public interface IEncoder<T>{
    byte[] Encode(T chunk);
}
