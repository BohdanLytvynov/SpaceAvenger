using System.Numerics;
using WPFGameEngine.WPF.GE.Geometry.Base;

namespace WPFGameEngine.WPF.GE.Geometry.Realizations
{
    public class Circle : Shape2D, IShape2D
    {
        public Vector2 Center { get; private set; }
        public float Radius { get; }

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public override void Translate(Vector2 offset)
        {
            Center = new Vector2(Center.X + offset.X, Center.Y + offset.Y);
        }

        public override Rectangle GetBounds()
        {
            return new Rectangle(
                Center.X - Radius,
                Center.Y - Radius,
                Radius * 2,
                Radius * 2);
        }
    }
}
