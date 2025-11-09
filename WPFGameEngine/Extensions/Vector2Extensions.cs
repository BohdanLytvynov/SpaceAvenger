using System.Drawing;
using System.Numerics;

namespace WPFGameEngine.Extensions
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

        public static double GetAngleDeg(this Vector2 l, Vector2 r)
        {
            var v = Vector2.Dot(l, r) / (l.Length() * r.Length());
            return Math.Acos(Math.Clamp(v, -1, 1)) * 180/Math.PI;
        }
    }
}
