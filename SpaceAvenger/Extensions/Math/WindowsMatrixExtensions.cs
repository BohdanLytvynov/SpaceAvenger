using System.Windows.Media;
using WPFGameEngine.WPF.GE.Math.Matrixes;

namespace SpaceAvenger.Extensions.Math
{
    /// <summary>
    /// Used to convert Matrix3x3 to the WPF Matrix for rendering
    /// </summary>
    public static class WindowsMatrixExtensions
    {
        /// <summary>
        /// Converts Matrix3x3 to the wpf Matrix
        /// </summary>
        /// <param name="m"></param>
        /// <param name="matrix"></param>
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
