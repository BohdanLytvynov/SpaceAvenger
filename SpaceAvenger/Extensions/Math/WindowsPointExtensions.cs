using System.Numerics;
using System.Windows;

namespace SpaceAvenger.Extensions.Math
{
    /// <summary>
    /// Used to create Windows.Point from the Vector2 class
    /// </summary>
    public static class WindowsPointExtensions
    {
        /// <summary>
        /// Build System.Windows.Point from the vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Point ToPoint(this Vector2 v)
        {
            return new Point(v.X, v.Y);
        }
    }
}
