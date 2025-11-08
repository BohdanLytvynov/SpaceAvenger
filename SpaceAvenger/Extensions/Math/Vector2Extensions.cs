using System.Drawing;
using System.Numerics;
using System.Windows;

namespace SpaceAvenger.Extensions.Math
{
    public static class Vector2Extensions
    {
        public static Vector2 Multiply(this Vector2 v, float number)
        {
            return new Vector2(v.X * number, v.Y * number);
        }

        public static System.Windows.Point ToPoint(this Vector2 v)
        {
            return new System.Windows.Point(v.X, v.Y);
        }

        public static Vector2 Multiply(this Vector2 v, SizeF size)
        {
            return new Vector2(v.X * size.Width, v.Y * size.Height);
        }
    }
}
