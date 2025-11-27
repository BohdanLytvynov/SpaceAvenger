using System.Numerics;
using System.Text.Json.Serialization;
using System.Windows.Media;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Attributes.Factories;
using WPFGameEngine.Enums;
using WPFGameEngine.WPF.GE.Geometry.Base;
using WPFGameEngine.WPF.GE.Math.Basis;
using WPFGameEngine.WPF.GE.Math.Sizes;
using WPFGameEngine.WPF.GE.Settings;

namespace WPFGameEngine.WPF.GE.Geometry.Realizations
{
    [VisibleInEditor(FactoryName = nameof(Rectangle),
        DisplayName = "Rectangle",
        GameObjectType = GEObjectType.Geometry)]
    [BuildWithFactory<GEObjectType>(GameObjectType = GEObjectType.Geometry)]
    public class Rectangle : Shape2D, IShape2D
    {
        [JsonIgnore]
        public Vector2 RightUpperCorner { get; private set; }// Right Upper Corner
        [JsonIgnore]
        public Vector2 LeftLowerCorner { get; private set; }// Left Lower Corner
        [JsonIgnore]
        public Vector2 RightLowerCorner { get; private set; }// Right Lower Corner

        public Size Size { get; set; }

        public Rectangle(Vector2 leftUpperCorner, Size scale, Basis2D basis)
        {
            LeftUpperCorner = leftUpperCorner;
            Scale = scale;
            Basis = basis;

            CalculatePoints();
        }

        public Rectangle()
        {
            
        }

        public override Rectangle GetBounds() => this;

        public override void Render(DrawingContext drawingContext)
        {
            base.Render(drawingContext);
            //Darw Horizontal Upper Line
            DrawLine(drawingContext,
                LeftUpperCorner,
                RightUpperCorner,
                GESettings.ColliderPointFillBrush,
                GESettings.ColliderPointPen,
                5,5, GESettings.ColliderBorderPen);
            //Draw Vertical Right Line
            DrawLine(drawingContext,
                RightUpperCorner,
                RightLowerCorner,
                GESettings.ColliderPointFillBrush,
                GESettings.ColliderPointPen,
                5, 5, GESettings.ColliderBorderPen);
            //Draw Horizontal Lower Line
            DrawLine(drawingContext,
                RightLowerCorner,
                LeftLowerCorner,
                GESettings.ColliderPointFillBrush,
                GESettings.ColliderPointPen,
                5, 5, GESettings.ColliderBorderPen);
            //Darw Vertical Right Line
            DrawLine(drawingContext,
                LeftLowerCorner,
                LeftUpperCorner,
                GESettings.ColliderPointFillBrush,
                GESettings.ColliderPointPen,
                5, 5, GESettings.ColliderBorderPen);
        }

        public override void CalculatePoints()
        {
            //We have to use Center Position
            var Xl = Basis.X * (Size.Width / 2 * Scale.Width);
            var Yl = Basis.Y * (Size.Height / 2 * Scale.Height);

            LeftUpperCorner = CenterPosition - (Xl + Yl);
            RightLowerCorner = CenterPosition + (Xl + Yl);
            RightUpperCorner = CenterPosition + (Xl - Yl);
            LeftLowerCorner = CenterPosition + (Yl - Xl);
        }

        public override List<Vector2> GetNormals() => new List<Vector2>()
        { 
            Basis.X, -Basis.X, Basis.Y, -Basis.Y
        };

        public override List<Vector2> GetVertexes() => 
            new List<Vector2>() { LeftUpperCorner, RightUpperCorner, LeftLowerCorner, RightLowerCorner };

        public override object Clone()
        {
            return new Rectangle() { CenterPosition = CenterPosition, Size = Size };
        }
    }
}
