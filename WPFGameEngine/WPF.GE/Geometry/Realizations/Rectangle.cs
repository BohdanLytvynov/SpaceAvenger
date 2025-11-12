using System.Numerics;
using WPFGameEngine.WPF.GE.Geometry.Base;

namespace WPFGameEngine.WPF.GE.Geometry.Realizations
{ 
    public class Rectangle : Shape2D, IShape2D
    {
        public Vector2 Min { get; private set; } // Левый верхний угол
        public float Width { get; }
        public float Height { get; }

        public Rectangle(float x, float y, float width, float height)
        {
            Min = new Vector2(x, y);
            Width = width;
            Height = height;
        }

        public override void Translate(Vector2 offset)
        {
            // Min += offset;
            Min = new Vector2(Min.X + offset.X, Min.Y + offset.Y);
        }

        public override Rectangle GetBounds() => this;
    }
}
