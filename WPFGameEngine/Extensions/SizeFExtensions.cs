using System.Drawing;

namespace WPFGameEngine.Extensions
{
    public static class SizeFExtensions
    {
        public static SizeF Multiply(this SizeF size, float scale)
        { 
            return new SizeF(size.Width * scale, size.Height * scale);
        }
    }
}
