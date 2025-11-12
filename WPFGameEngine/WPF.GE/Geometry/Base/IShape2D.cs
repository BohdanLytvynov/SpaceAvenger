using System.Numerics;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.WPF.GE.Geometry.Realizations;

namespace WPFGameEngine.WPF.GE.Geometry.Base
{
    public interface IShape2D
    {
        void Translate(Vector2 offset);
        bool IntersectsWith(IShape2D other);
        Rectangle GetBounds();
    }
}
