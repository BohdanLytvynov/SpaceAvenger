using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.WPF.GE.Geometry.Realizations;
using WPFGameEngine.WPF.GE.Helpers;

namespace WPFGameEngine.WPF.GE.Geometry.Base
{
    public abstract class Shape2D : IShape2D
    {
        public abstract Rectangle GetBounds();
        public bool IntersectsWith(IShape2D other)
        { 
            return CollisionHelper.Intersects(this, other);
        }
        public abstract void Render(DrawingContext drawingContext, Matrix parent);
        public abstract void Translate(Vector2 offset);
    }
}
