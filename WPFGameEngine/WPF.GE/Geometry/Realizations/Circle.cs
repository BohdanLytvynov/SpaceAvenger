using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Attributes.Factories;
using WPFGameEngine.Enums;
using WPFGameEngine.WPF.GE.Geometry.Base;
using WPFGameEngine.WPF.GE.Math.Sizes;
using WPFGameEngine.WPF.GE.Settings;

namespace WPFGameEngine.WPF.GE.Geometry.Realizations
{
    [VisibleInEditor(FactoryName = nameof(Circle),
        DisplayName = "Circle",
        GameObjectType = GEObjectType.Geometry)]
    [BuildWithFactory<GEObjectType>(GameObjectType = GEObjectType.Geometry)]
    public class Circle : Shape2D, IShape2D
    {
        public float Radius { get; set; }

        public override Rectangle GetBounds()
        {
            return new Rectangle(
                LeftUpperCorner,
                new Size(Radius * 2, Radius * 2),
                Basis);
        }

        public Circle()
        {
            
        }

        public override List<Vector2> GetNormals() => new List<Vector2>();//Infinite number of Normals

        public override void Render(DrawingContext drawingContext)
        {
            base.Render(drawingContext);
            drawingContext.DrawEllipse(GESettings.ColliderFillBrush, GESettings.ColliderBorderPen,
                    new System.Windows.Point(CenterPosition.X, CenterPosition.Y),
                    Radius * Scale.Width, Radius * Scale.Height);
        }

        public override void CalculatePoints()
        {
            //NoPoints to Calculate
        }

        public override List<Vector2> GetVertexes() => new List<Vector2>() {};
    }
}
