using System.Numerics;
using WPFGameEngine.WPF.GE.Geometry.Base;
using SMath = System.Math;

namespace WPFGameEngine.WPF.GE.Geometry.Realizations
{
    public class Triangle : Shape2D, IShape2D
    {
        public Vector2 A { get; private set; }
        public Vector2 B { get; private set; }
        public Vector2 C { get; private set; }

        public Triangle(Vector2 a, Vector2 b, Vector2 c)
        {
            A = a;
            B = b;
            C = c;
        }

        public override void Translate(Vector2 offset)
        {
            // A += offset; B += offset; C += offset;
            A = new Vector2(A.X + offset.X, A.Y + offset.Y);
            B = new Vector2(B.X + offset.X, B.Y + offset.Y);
            C = new Vector2(C.X + offset.X, C.Y + offset.Y);
        }

        public override Rectangle GetBounds()
        {
            // Расчет минимального AABB для треугольника
            float minX = SMath.Min(A.X, SMath.Min(B.X, C.X));
            float minY = SMath.Min(A.Y, SMath.Min(B.Y, C.Y));
            float maxX = SMath.Max(A.X, SMath.Max(B.X, C.X));
            float maxY = SMath.Max(A.Y, SMath.Max(B.Y, C.Y));

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }
    }
}
