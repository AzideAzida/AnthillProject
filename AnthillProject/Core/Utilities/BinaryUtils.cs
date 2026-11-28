namespace AnthillProject.Core.Utilities;
public static class BinaryUtils{
    public static int IntFromBytesBE(byte[] bytes){//https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/how-to-convert-a-byte-array-to-an-int
        if(BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        //Console.WriteLine(BitConverter.ToInt32(bytes, 0));
        return BitConverter.ToInt32(bytes, 0);
    }
    public static ushort UShortFromBytesBE(byte[] bytes){
        if(BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        return BitConverter.ToUInt16(bytes, 0);
    }
    public static float FloatFromBytesBE(byte[] bytes){
        if(BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        return BitConverter.ToSingle(bytes, 0);
    }


    public static byte[] GetBytesBE(int i){//https://stackoverflow.com/a/1318948
        byte[] bytes = BitConverter.GetBytes(i);
        if(BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        return bytes;
    }
    public static byte[] GetBytesBE(ushort i){
        byte[] bytes = BitConverter.GetBytes(i);
        if(BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        return bytes;
    }
    public static byte[] GetBytesBE(float i){
        byte[] bytes = BitConverter.GetBytes(i);
        if(BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        return bytes;
    }
}