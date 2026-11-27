using SkiaSharp;
using AnthillProject.Core.Persistent.Saving;
using System.Reflection;

namespace AnthillProject.Core;
public static class Utility{
    static Utility(){
        AnthillProjectAssembly = typeof(Program).Assembly;
        ManifestResourceNames = AnthillProjectAssembly.GetManifestResourceNames();
    }
    public readonly static Assembly AnthillProjectAssembly;
    public static string[] ManifestResourceNames{
        get =>
            (string[])field.Clone();
        private set;
    }
    public static SKBitmap RetrieveTexture(string name){
        using var stream = AnthillProjectAssembly.GetManifestResourceStream($"AnthillProject.Resources.Textures.{name}");
        return SKBitmap.Decode(stream);
    }
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
    public static void DoNothing(object? i){}
    public static byte[] ChangeChunkCodec(byte[] source, IChunkCodec sourceCodec, IChunkCodec destCodec) =>
        destCodec.Encode(sourceCodec.Decode(source));
    
    public static long GetMilliseconds() => DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    
    //Idfk what else to name this. Margin is inclusive.
    public static float RoundIfCloseToBorders(float Base, float Margin){
        if(GetDecimals(Base) <= Margin)
            return MathF.Floor(Base);
        
        var BaseCeel  = MathF.Ceiling(Base);
        if(BaseCeel - Base <= Margin)
            return BaseCeel;

        return Base;
    }
    public static float GetDecimals(float i) =>
        i % 1;

    public static SKBitmap ResizeImage(SKBitmap image, int width, int height) =>
        image.Resize(new SKImageInfo(width, height), SKSamplingOptions.Default);
    
    
    public static SKBitmap ArgbBitmap1x1(byte red, byte green, byte blue, byte alpha = 255) =>
        ColorBitmap1x1(new SKColor(red, green, blue, alpha));
    public static SKBitmap ColorBitmap1x1(SKColor i){
        var Bmp = new SKBitmap(1, 1);
        Bmp.SetPixel(0, 0, i);
        return Bmp;
    }

    //If the provided object is null or doesn't implement IDisposable, does nothing and returns false.
    //Otherwise, it disposes it and returns true.
    public static bool TryDispose(object? obj){
        if(obj == null)
            return false;

        if(obj is IDisposable disposable){
            disposable.Dispose();
            return true;
        }
        return false;
    }
    
    internal static void TestSpeed(){
        //var image = SkiaSharp.Image.FromFile(@"");
        //Console.WriteLine(GetMilliseconds());
        //for(int i = 0; i < 60; i++){
        //    Utility.SystemImageToEtoBitmap(image);
        //}
        //Console.WriteLine(GetMilliseconds());
    }
}
