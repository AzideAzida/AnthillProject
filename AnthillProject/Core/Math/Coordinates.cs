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
    public override int GetHashCode() => base.GetHashCode();
    public override bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);

    #region Operations.
        #region Operations with other Coordinates.
            #region Static methods.
                public static Coordinates Add     (Coordinates i, Coordinates e) => new Coordinates(i.x + e.x, i.y + e.y);
                public static Coordinates Subtract(Coordinates i, Coordinates e) => new Coordinates(i.x - e.x, i.y - e.y);
                public static Coordinates Multiply(Coordinates i, Coordinates e) => new Coordinates(i.x * e.x, i.y * e.y);
                public static Coordinates Divide  (Coordinates i, Coordinates e) => new Coordinates(i.x / e.x, i.y / e.y);
            #endregion
            #region Instance Methods.
                public Coordinates Add     (Coordinates i) => Add     (this, i);//Addition.
                public Coordinates Subtract(Coordinates i) => Subtract(this, i);//Subtraction.
                public Coordinates Multiply(Coordinates i) => Multiply(this, i);//Multiplication.
                public Coordinates Divide  (Coordinates i) => Divide  (this, i);//Division.
            #endregion
            #region Operators.
                public static Coordinates operator +(Coordinates This, Coordinates i) => Add     (This, i);//Addition.
                public static Coordinates operator -(Coordinates This, Coordinates i) => Subtract(This, i);//Subtraction.
                public static Coordinates operator *(Coordinates This, Coordinates i) => Multiply(This, i);//Multiplication.
                public static Coordinates operator /(Coordinates This, Coordinates i) => Divide  (This, i);//Division.
            #endregion
        #endregion

        #region Operations with ints.
            #region Static methods.
                public static Coordinates Add     (Coordinates i, int e) => new Coordinates(i.x + e, i.y + e);
                public static Coordinates Subtract(Coordinates i, int e) => new Coordinates(i.x - e, i.y - e);
                public static Coordinates Multiply(Coordinates i, int e) => new Coordinates(i.x * e, i.y * e);
                public static Coordinates Divide  (Coordinates i, int e) => new Coordinates(i.x / e, i.y / e);
            #endregion
            #region Instance Methods.
                public Coordinates Add     (int i) => Add     (this, i);
                public Coordinates Subtract(int i) => Subtract(this, i);
                public Coordinates Multiply(int i) => Multiply(this, i);
                public Coordinates Divide  (int i) => Divide  (this, i);
            #endregion
            #region Operators.
                public static Coordinates operator +(Coordinates This, int   i) => new Coordinates(This.x + i  , This.y + i  );//Addition.
                public static Coordinates operator -(Coordinates This, int   i) => new Coordinates(This.x - i  , This.y - i  );//Subtraction.
                public static Coordinates operator *(Coordinates This, int   i) => new Coordinates(This.x * i  , This.y * i  );//Multiplication.
                public static Coordinates operator /(Coordinates This, int   i) => new Coordinates(This.x / i  , This.y / i  );//Division.
            #endregion
        #endregion
        public static bool operator ==(Coordinates left, Coordinates right) =>
            left.Equals(right);

        public static bool operator !=(Coordinates left, Coordinates right) =>
            !(left == right);
    #endregion
}