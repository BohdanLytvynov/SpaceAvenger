using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Geometry.Realizations;
using WPFGameEngine.WPF.GE.Math.Basis;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.Geometry.Base
{
    public interface IShape2D : IGameEngineEntity
    {
        public Size Scale { get; set; }
        public Basis2D Basis { get; set; }
        public Vector2 CenterPosition { get; set; }
        public Vector2 LeftUpperCorner { get; set; }
        void Render(DrawingContext drawingContext);
        bool IntersectsWith(IShape2D other);
        Rectangle GetBounds();
        List<Vector2> GetNormals();
    }
}
