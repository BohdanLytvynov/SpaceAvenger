

using System.Numerics;
using System.Windows;

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
    }
}
