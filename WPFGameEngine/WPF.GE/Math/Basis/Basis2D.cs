using System.Numerics;

namespace WPFGameEngine.WPF.GE.Math.Basis
{
    public struct Basis2D
    {
        public Vector2 X { get; set; }

        public Vector2 Y { get; set; }

        public Basis2D(Vector2 x, Vector2 y)
        {
            X = x;
            Y = y;
        }

        public Vector2 GetNormalX()
        { 
            var length = X.Length();
            return X * (1 / length);
        }

        public Vector2 GetNormalY()
        {
            var length = Y.Length();
            return Y * (1 / length);
        }
    }
}
