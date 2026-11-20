#pragma warning disable CS8600

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using AnthillProject.Core.Persistent.Saving;

namespace AnthillProject.Core;
public static class Utility{
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

    public static Bitmap ResizeImage(Image image, int width, int height){ //https://stackoverflow.com/a/24199315
        var destRect = new Rectangle(0, 0, width, height);
        var destImage = new Bitmap(width, height);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using(var graphics = Graphics.FromImage(destImage)){
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using(var wrapMode = new ImageAttributes()){
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width,image.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }

        return destImage;
    }
    
    public static Bitmap ArgbBitmap1x1(int red, int green, int blue, int alpha = 255) =>
        ColorBitmap1x1(Color.FromArgb(alpha, red, green, blue));
    public static Bitmap ColorBitmap1x1(Color i){
        var Bmp = new Bitmap(1, 1);
        Bmp.SetPixel(0, 0, i);
        return Bmp;
    }
    public static Eto.Drawing.Bitmap SystemImageToEtoBitmap(Image img){
        return new Eto.Drawing.Bitmap(
            (byte[])new ImageConverter().ConvertTo(img, typeof(byte[]))
        );
    }
    internal static void TestSpeed(){
        //var image = System.Drawing.Image.FromFile(@"");
        //Console.WriteLine(GetMilliseconds());
        //for(int i = 0; i < 60; i++){
        //    Utility.SystemImageToEtoBitmap(image);
        //}
        //Console.WriteLine(GetMilliseconds());
    }
}
