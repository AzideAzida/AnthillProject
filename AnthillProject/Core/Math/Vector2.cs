using System.Diagnostics.CodeAnalysis;

namespace AnthillProject.Core.Math;

public readonly struct Vector2{
    public static readonly Vector2 Zero = new Vector2();
    public readonly float x, y;
    public override string ToString() =>
        $"({x}, {y})";
    public Vector2(){}
    public Vector2(float i){
        x = i;
        y = i;
    }
    public Vector2(float X, float Y){
        x = X;
        y = Y;
    }
    public override int GetHashCode() =>
        new {x, y}.GetHashCode();
    public override bool Equals([NotNullWhen(true)] object? obj){
        if (obj is Vector2 i)
            return x == i.x && y == i.y;
        return false;
    }
    
    //Operations with other Vector2s.
    public static Vector2 operator +(Vector2 e, Vector2 i) => new Vector2(i.x + e.x, i.y + e.y);//Addition.
    public static Vector2 operator -(Vector2 e, Vector2 i) => new Vector2(i.x - e.x, i.y - e.y);//Subtraction.
    public static Vector2 operator *(Vector2 e, Vector2 i) => new Vector2(i.x * e.x, i.y * e.y);//Multiplication.
    public static Vector2 operator /(Vector2 e, Vector2 i) => new Vector2(i.x / e.x, i.y / e.y);//Division.

    //Operations with floats.
    public static Vector2 operator +(Vector2 e, float   i) => new Vector2(e.x + i  , e.y + i  );//Addition.
    public static Vector2 operator -(Vector2 e, float   i) => new Vector2(e.x - i  , e.y - i  );//Subtraction.
    public static Vector2 operator *(Vector2 e, float   i) => new Vector2(e.x * i  , e.y * i  );//Multiplication.
    public static Vector2 operator /(Vector2 e, float   i) => new Vector2(e.x / i  , e.y / i  );//Division.
    
    public static bool operator ==(Vector2 left, Vector2 right) =>
        left.Equals(right);

    public static bool operator !=(Vector2 left, Vector2 right) =>
        !(left == right);
}