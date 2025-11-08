using System.Numerics;
using System.Windows;
using System.Windows.Media;
using WPFGameEngine.WPF.GE.Math.Basis;

namespace WPFGameEngine.Extensions
{
    public static class MatrixExtensions
    {
        public static void CheckMachineZero(this ref Matrix matrix)
        { 
            var eps = GEConstants.Epsilon;
            if (Math.Abs(matrix.M11) < eps)
                matrix.M11 = 0.0;
            if(Math.Abs(matrix.M12) < eps)
                matrix.M12 = 0.0;
            if(Math.Abs(matrix.M21) < eps)
                matrix.M21 = 0.0;
            if(Math.Abs(matrix.M22) < eps)
                matrix.M22 = 0.0;
        }

        public static Point GetTranslate(this Matrix matrix)
        {
            return new Point(matrix.OffsetX, matrix.OffsetY);
        }

        public static Vector2 GetTranslateAsVector(this Matrix matrix)
        {
            return new Vector2((float)matrix.OffsetX, (float)matrix.OffsetY);
        }

        public static Basis2D GetBasis(this Matrix matrix)
        {
            return new Basis2D()
            {
                X = Vector2.Normalize(new Vector2((float)matrix.M11, (float)matrix.M12)),
                Y = Vector2.Normalize(new Vector2((float)matrix.M21, (float)matrix.M22))
            };
        }
    }
}
