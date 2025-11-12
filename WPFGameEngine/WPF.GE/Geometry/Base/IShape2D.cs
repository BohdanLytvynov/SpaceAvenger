using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.WPF.GE.Geometry.Realizations;

namespace WPFGameEngine.WPF.GE.Geometry.Base
{
    public interface IShape2D
    {
        void Render(DrawingContext drawingContext, Matrix parent);
        void Translate(Vector2 offset);
        bool IntersectsWith(IShape2D other);
        Rectangle GetBounds();
    }
}
