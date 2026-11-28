using System.Reflection;
using SkiaSharp;

namespace AnthillProject.Core.Utilities;
public static class ReflectionUtils{
    static ReflectionUtils(){
        AnthillProjectAssembly = typeof(Program).Assembly;
        ManifestResourceNames = AnthillProjectAssembly.GetManifestResourceNames();
    }
    public readonly static Assembly AnthillProjectAssembly;
    public static string[] ManifestResourceNames{
        get =>
            (string[])field.Clone();
        private set;
    }

    //This is considered a reflection utility because this retrieves embedded resources from the assembly.
    public static SKBitmap RetrieveTexture(string name){
        using var stream = AnthillProjectAssembly.GetManifestResourceStream($"AnthillProject.Resources.Textures.{name}");
        return SKBitmap.Decode(stream);
    }
}