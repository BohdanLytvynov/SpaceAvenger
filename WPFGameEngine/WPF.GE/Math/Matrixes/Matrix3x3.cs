using System.Configuration;
using System.Numerics;
using WPFGameEngine.WPF.GE.Math.Basis;
using WPFGameEngine.WPF.GE.Math.Sizes;
using SMath = System.Math;

namespace WPFGameEngine.WPF.GE.Math.Matrixes
{
    public struct Matrix3x3
    {
        /// <summary>
        ///     0     1    2
        /// 0 {M11,  M12,  0},
        /// 1 {M21,  M22,  0},
        /// 2 {OffX, OffY, 1}
        /// M11 - ScaleX,
        /// M22 - ScaleY
        /// </summary>
        private float[,] m_Matrix;

        public float M11 { get => this[0,0]; }
        public float M12 { get => this[0,1]; }
        public float M21 { get => this[1,0]; }
        public float M22 { get => this[1,1]; }
        public float OffsetX { get => this[2,0]; }
        public float OffsetY { get => this[2,1]; }
        public static Matrix3x3 Identity { get => new Matrix3x3(
            1,0,0,
            0,1,0,
            0,0,1); }
        public int Rows { get => 3; }
        public int Columns { get => 3; }

        public Matrix3x3()
        {
            m_Matrix = new float[3, 3]
                {
                    { 1, 0, 0 },
                    { 0, 1, 0 },
                    { 0, 0, 1 },
                };
        }

        Matrix3x3(float[,] matrix)
        { 
            if(matrix == null)
                throw new ArgumentNullException(nameof(matrix));

            m_Matrix = new float[3, 3];

            int r = matrix.GetLength(0);
            int c = matrix.GetLength(1);

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    m_Matrix[i,j] = matrix[i,j];
                }
            }
        }

        public Matrix3x3(
            float m11, float m12, float m13,
            float m21, float m22, float m23,
            float m31, float m32, float m33)
        {
            m_Matrix = new float[3, 3];
            m_Matrix[0,0] = m11;
            m_Matrix[0,1] = m12;
            m_Matrix[0,2] = m13;

            m_Matrix[1,0] = m21;
            m_Matrix[1,1] = m22;
            m_Matrix[1,2] = m23;

            m_Matrix[2,0] = m31;
            m_Matrix[2,1] = m32;
            m_Matrix[2,2] = m33;
        }

        public void Translate(Vector2 position)
        {
            float[,] tm = new float[3, 3]
            {
                { 1f,         0f,         0f },
                { 0f,         1f,         0f },
                { position.X, position.Y, 1f }
            };

            m_Matrix = Multiply(m_Matrix, tm);
        }
        /// <summary>
        /// angle in Degrees
        /// </summary>
        /// <param name="angle"></param>
        public void Rotate(double angle)
        {
            double angleRad = angle * SMath.PI / 180;
            //Build Rot Matrix
            float cos = (float)SMath.Cos(angleRad);
            float sin = (float)SMath.Sin(angleRad);

            float[,] rm = new float[3, 3]
            {
                {  cos,  sin, 0f },
                { -sin,  cos, 0f },
                {  0f,   0f,  1f }
            };

            m_Matrix = Multiply(m_Matrix, rm);
        }

        public void Scale(Size size)
        {
            float[,] sm = new float[3, 3]
            {
                { size.Width, 0f,          0f },
                { 0f,         size.Height, 0f },
                { 0f,         0f,          1f }
            };

            m_Matrix = Multiply(m_Matrix, sm);
        }

        public float this[int i, int j]
        { 
            get => m_Matrix[i,j];
            set => m_Matrix[i,j] = value;
        }

        public static Matrix3x3 operator * (Matrix3x3 l, Matrix3x3 r)
        {
            int Rl = l.Rows;
            int Cl = l.Columns;
            int Rr = r.Rows;
            int Cr = r.Columns;

            Matrix3x3 R = new Matrix3x3();

            for (int i = 0; i < Rl; i++)//O(N^3) => 729
            {
                for (int j = 0; j < Cr; j++)
                {
                    float sum = 0f;
                    for (int k = 0; k < Cl; k++)
                    {
                        sum += l[i, k] * r[k, j];
                    }
                    R[i, j] = sum;
                }
            }

            return R;
        }

        public static bool operator ==(Matrix3x3 l, Matrix3x3 r)
        {
            int rows = l.Rows;
            int columns = l.Columns;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (l[i,j] != r[i,j])
                        return false;
                }
            }

            return true;

        }

        public static bool operator !=(Matrix3x3 l, Matrix3x3 r) => !(l == r);

        public Basis2D GetBasis() =>
            new Basis2D(CheckNormalize(new Vector2(M11, M12)), CheckNormalize(new Vector2(M21, M22)));

        public Vector2 GetTranslate() => new Vector2(m_Matrix[2, 0], m_Matrix[2, 1]);

        public void CheckMachineZero()
        {
            var eps = GEConstants.Epsilon;
            if (SMath.Abs(m_Matrix[0, 0]) < eps)
                m_Matrix[0, 0] = 0.0f;
            if (SMath.Abs(m_Matrix[0, 1]) < eps)
                m_Matrix[0, 1] = 0.0f;
            if (SMath.Abs(m_Matrix[1, 0]) < eps)
                m_Matrix[1, 0] = 0.0f;
            if (SMath.Abs(m_Matrix[1, 1]) < eps)
                m_Matrix[1, 1] = 0.0f;
            if (SMath.Abs(m_Matrix[2, 0]) < eps)
                m_Matrix[2, 0] = 0.0f;
            if (SMath.Abs(m_Matrix[2, 1]) < eps)
                m_Matrix[2, 1] = 0.0f;
        }

        public Size GetScaleFactors()
        {
            return new Size(
                MathF.Sqrt(M11*M11 + M21*M21),
                MathF.Sqrt(M12*M12 + M22*M22)
                );
        }

        #region Private Methods

        private static float[,] Multiply(float[,] l, float[,] r)
        {
            int Rl = l.GetLength(0);
            int Cl = l.GetLength(1);
            int Rr = r.GetLength(0);
            int Cr = r.GetLength(1);
            float[,] res = new float[Rl, Cr];
            for (int i = 0; i < Rl; i++)//O(N^3) => 729
            {
                for (int j = 0; j < Cr; j++)
                {
                    float sum = 0f;
                    for (int k = 0; k < Cl; k++)
                    {
                        sum += l[i, k] * r[k, j];
                    }
                    res[i, j] = sum;
                }
            }
            return res;
        }

        private static Vector2 CheckNormalize(Vector2 input)
        {
            if (input.Length() != 1.0f)
                return Vector2.Normalize(input);
            return input;
        }

        #endregion

    }
}
