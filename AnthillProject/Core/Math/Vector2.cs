using System.Diagnostics.CodeAnalysis;

namespace AnthillProject.Core.Math;
public readonly struct Vector2{
    public readonly float x, y;
    
    public Vector2(){}
    public Vector2(float i){
        x = i;
        y = i;
    }
    public Vector2(float X, float Y){
        x = X;
        y = Y;
    }
    public Vector2((float X, float Y) i){
        x = i.X;
        y = i.Y;
    }
    public static Vector2 FromFloatCastableObjects(object X, object Y) =>
        new Vector2(
            (float)X,
            (float)Y
        );

    public override string ToString() =>
        $"({x}, {y})";
    public override int GetHashCode() => base.GetHashCode();
    public override bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);
    
    #region Operations.
        #region Operations with other Vector2s.
            #region Static methods.
                public static Vector2 Add     (Vector2 i, Vector2 e) => new Vector2(i.x + e.x, i.y + e.y);
                public static Vector2 Subtract(Vector2 i, Vector2 e) => new Vector2(i.x - e.x, i.y - e.y);
                public static Vector2 Multiply(Vector2 i, Vector2 e) => new Vector2(i.x * e.x, i.y * e.y);
                public static Vector2 Divide  (Vector2 i, Vector2 e) => new Vector2(i.x / e.x, i.y / e.y);
            #endregion
            #region Instance Methods.
                public Vector2 Add     (Vector2 i) => Add     (this, i);//Addition.
                public Vector2 Subtract(Vector2 i) => Subtract(this, i);//Subtraction.
                public Vector2 Multiply(Vector2 i) => Multiply(this, i);//Multiplication.
                public Vector2 Divide  (Vector2 i) => Divide  (this, i);//Division.
            #endregion
            #region Operators.
                public static Vector2 operator +(Vector2 This, Vector2 i) => Add     (This, i);//Addition.
                public static Vector2 operator -(Vector2 This, Vector2 i) => Subtract(This, i);//Subtraction.
                public static Vector2 operator *(Vector2 This, Vector2 i) => Multiply(This, i);//Multiplication.
                public static Vector2 operator /(Vector2 This, Vector2 i) => Divide  (This, i);//Division.
            #endregion
        #endregion

        #region Operations with floats.
            #region Static methods.
                public static Vector2 Add     (Vector2 i, float e) => new Vector2(i.x + e, i.y + e);
                public static Vector2 Subtract(Vector2 i, float e) => new Vector2(i.x - e, i.y - e);
                public static Vector2 Multiply(Vector2 i, float e) => new Vector2(i.x * e, i.y * e);
                public static Vector2 Divide  (Vector2 i, float e) => new Vector2(i.x / e, i.y / e);
            #endregion
            #region Instance Methods.
                public Vector2 Add     (float i) => Add     (this, i);
                public Vector2 Subtract(float i) => Subtract(this, i);
                public Vector2 Multiply(float i) => Multiply(this, i);
                public Vector2 Divide  (float i) => Divide  (this, i);
            #endregion
            #region Operators.
                public static Vector2 operator +(Vector2 This, float   i) => new Vector2(This.x + i  , This.y + i  );//Addition.
                public static Vector2 operator -(Vector2 This, float   i) => new Vector2(This.x - i  , This.y - i  );//Subtraction.
                public static Vector2 operator *(Vector2 This, float   i) => new Vector2(This.x * i  , This.y * i  );//Multiplication.
                public static Vector2 operator /(Vector2 This, float   i) => new Vector2(This.x / i  , This.y / i  );//Division.
            #endregion
        #endregion
        public static bool operator ==(Vector2 left, Vector2 right) =>
            left.Equals(right);

        public static bool operator !=(Vector2 left, Vector2 right) =>
            !(left == right);
    #endregion
}
