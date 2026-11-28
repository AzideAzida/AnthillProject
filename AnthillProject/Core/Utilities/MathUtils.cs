namespace AnthillProject.Core.Utilities;
public static class MathUtils{
    public static long GetMilliseconds() => DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    
    //If the input is negative, then the output will be aswell.
    public static float GetDecimals(float i){
        if(i < 0)
            return -(-i % 1);
        return i % 1;
    }
}