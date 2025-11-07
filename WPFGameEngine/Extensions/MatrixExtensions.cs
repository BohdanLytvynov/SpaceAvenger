using System.Windows.Media;

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
    }
}
