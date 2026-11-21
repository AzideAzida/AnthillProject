using System.Diagnostics.CodeAnalysis;

namespace AnthillProject.Core.Math;
//Basically a Vector2 but with integers.
public readonly struct Coordinates{
    public static readonly Coordinates Zero = new Coordinates();
    public readonly int x, y;
    public override string ToString() =>
        $"({x}, {y})";
    public Coordinates(){}
    public Coordinates(int i){
        x = i;
        y = i;
    }
    public Coordinates(int X, int Y){
        x = X;
        y = Y;
    }
    public override int GetHashCode() =>
        new {x, y}.GetHashCode();
    public override bool Equals([NotNullWhen(true)] object? obj){
        if (obj is Coordinates i)
            return x == i.x && y == i.y;
        return false;
    }

    //Operations with other Coordinates.
    public static Coordinates operator +(Coordinates e, Coordinates i) => new Coordinates(i.x + e.x, i.y + e.y);//Addition.
    public static Coordinates operator -(Coordinates e, Coordinates i) => new Coordinates(i.x - e.x, i.y - e.y);//Subtraction.
    public static Coordinates operator *(Coordinates e, Coordinates i) => new Coordinates(i.x * e.x, i.y * e.y);//Multiplication.
    public static Coordinates operator /(Coordinates e, Coordinates i) => new Coordinates(i.x / e.x, i.y / e.y);//Division.

    //Operations with integers.
    public static Coordinates operator +(Coordinates e, int   i) => new Coordinates(e.x + i , e.y + i  );//Addition.
    public static Coordinates operator -(Coordinates e, int   i) => new Coordinates(e.x - i , e.y - i  );//Subtraction.
    public static Coordinates operator *(Coordinates e, int   i) => new Coordinates(e.x * i , e.y * i  );//Multiplication.
    public static Coordinates operator /(Coordinates e, int   i) => new Coordinates(e.x / i , e.y / i  );//Division.

    public static bool operator ==(Coordinates left, Coordinates right) =>
        left.Equals(right);

    public static bool operator !=(Coordinates left, Coordinates right) =>
        !(left == right);
}