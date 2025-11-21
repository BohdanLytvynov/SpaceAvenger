using System.Numerics;
using System.Windows;

namespace SpaceAvenger.Extensions.Math
{
    public static class WindowsPointExtensions
    {
        public static Point FromVector2(this Vector2 vector)
        { 
            return new Point(vector.X, vector.Y);
        }
    }
}
