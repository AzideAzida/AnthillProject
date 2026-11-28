namespace AnthillProject.Core.Utilities;
public static class MathUtils{
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
}