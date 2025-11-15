using System.Numerics;
using System.Text.Json.Serialization;
using System.Windows.Media;
using WPFGameEngine.WPF.GE.Geometry.Realizations;
using WPFGameEngine.WPF.GE.Helpers;
using WPFGameEngine.WPF.GE.Math.Basis;
using WPFGameEngine.WPF.GE.Math.Sizes;
using WPFGameEngine.WPF.GE.Settings;

namespace WPFGameEngine.WPF.GE.Geometry.Base
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "RecreateType")]
    [JsonDerivedType(typeof(Circle), nameof(Circle))]
    public abstract class Shape2D : IShape2D
    {
        [JsonIgnore]
        public Vector2 CenterPosition { get; set; }
        [JsonIgnore]
        public Vector2 LeftUpperCorner { get; set; }
        [JsonIgnore]
        public Basis2D Basis { get; set; }
        [JsonIgnore]
        public Size Scale { get; set; }

        public bool IntersectsWith(IShape2D other)
        { 
            return CollisionHelper.Intersects(this, other);
        }
        //To do -> need to be moved to Wpf
        public virtual void Render(DrawingContext drawingContext)
        {
            CalculatePoints();
            drawingContext.DrawEllipse(GESettings.ColliderPointFillBrush, GESettings.ColliderPointPen,
                   new System.Windows.Point(CenterPosition.X, CenterPosition.Y),
                   5, 5);
        }
        public abstract Rectangle GetBounds();
        public abstract List<Vector2> GetNormals();
        protected abstract void CalculatePoints();
        protected void DrawLine(DrawingContext drawingContext, Vector2 start, Vector2 end,
            Brush PointFillBrush, Pen PointBorderPen, double pointRadx, double pointRady, Pen linePen)
        {
            drawingContext.DrawEllipse(PointFillBrush, PointBorderPen,
                   new System.Windows.Point(end.X, end.Y),
                   pointRadx, pointRady);

            drawingContext.DrawLine(linePen,
                new System.Windows.Point(
                start.X,
                start.Y),
                new System.Windows.Point(
                end.X,
                end.Y));
        }
    }
}
