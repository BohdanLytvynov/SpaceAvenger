using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Attributes.Factories;
using WPFGameEngine.Enums;
using WPFGameEngine.WPF.GE.Geometry.Base;
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

        //public override void Translate(Vector2 offset)
        //{
        //    CenterPosition = new Vector2(CenterPosition.X + offset.X, CenterPosition.Y + offset.Y);
        //}

        //public override Rectangle GetBounds()
        //{
        //    return new Rectangle(
        //        CenterPosition.X - Radius,
        //        CenterPosition.Y - Radius,
        //        Radius * 2,
        //        Radius * 2);
        //}

        public override void Render(DrawingContext drawingContext)
        {
            drawingContext.DrawEllipse(GESettings.ColliderFillBrush, GESettings.ColliderBorderPen,
                    new System.Windows.Point(CenterPosition.X, CenterPosition.Y),
                    Radius * Scale.Width, Radius * Scale.Height);

            drawingContext.DrawEllipse(GESettings.ColliderPointFillBrush, GESettings.ColliderPointPen,
                    new System.Windows.Point(CenterPosition.X, CenterPosition.Y),
                    5, 5);
        }
    }
}
