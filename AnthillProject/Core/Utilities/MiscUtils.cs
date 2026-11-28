using AnthillProject.Core.Persistent.Saving;

namespace AnthillProject.Core.Utilities;
public static class MiscUtils{
    public static void DoNothing(object? i){}
    public static byte[] ChangeChunkCodec(byte[] source, IChunkCodec sourceCodec, IChunkCodec destCodec) =>
        destCodec.Encode(sourceCodec.Decode(source));

    

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
