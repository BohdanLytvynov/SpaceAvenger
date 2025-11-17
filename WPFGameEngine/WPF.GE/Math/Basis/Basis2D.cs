

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

        public bool IsIdentity() =>
            X.X == 1 && X.Y == 0 && Y.X == 0 && Y.Y == 1;
    }
}
