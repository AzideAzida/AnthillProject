using SkiaSharp;
namespace AnthillProject.Core.Utilities;
public static class GraphicUtils{
    public static SKBitmap ResizeImage(SKBitmap image, int width, int height) =>
        image.Resize(new SKImageInfo(width, height), SKSamplingOptions.Default);
    
    
    public static SKBitmap ArgbBitmap1x1(byte red, byte green, byte blue, byte alpha = 255) =>
        ColorBitmap1x1(new SKColor(red, green, blue, alpha));
    public static SKBitmap ColorBitmap1x1(SKColor i){
        var Bmp = new SKBitmap(1, 1);
        Bmp.SetPixel(0, 0, i);
        return Bmp;
    }
}