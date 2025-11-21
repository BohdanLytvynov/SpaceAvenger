using System.Windows.Media;
using WPFGameEngine.WPF.GE.Math.Matrixes;

namespace SpaceAvenger.Extensions.Math
{
    public static class WindowsMatrixExtensions
    {
        public static void BuildWindowMatrix(this ref Matrix m, Matrix3x3 matrix)
        {
            m.M11 = matrix.M11;
            m.M12 = matrix.M12;
            m.M21 = matrix.M21;
            m.M22 = matrix.M22;
            m.OffsetX = matrix.OffsetX;
            m.OffsetY = matrix.OffsetY;
        }
    }
}
