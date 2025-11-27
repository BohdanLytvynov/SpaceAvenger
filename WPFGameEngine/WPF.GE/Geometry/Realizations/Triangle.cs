using System.Numerics;
using System.Text.Json.Serialization;
using System.Windows.Controls;
using System.Windows.Media;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Attributes.Factories;
using WPFGameEngine.Enums;
using WPFGameEngine.Extensions;
using WPFGameEngine.WPF.GE.Geometry.Base;
using WPFGameEngine.WPF.GE.Math.Sizes;
using WPFGameEngine.WPF.GE.Settings;
using SMath = System.Math;

namespace WPFGameEngine.WPF.GE.Geometry.Realizations
{
    [VisibleInEditor(FactoryName = nameof(Triangle),
        DisplayName = "Triangle",
        GameObjectType = GEObjectType.Geometry)]
    [BuildWithFactory<GEObjectType>(GameObjectType = GEObjectType.Geometry)]
    public class Triangle : Shape2D, IShape2D
    {
        [JsonIgnore]
        public Vector2 B { get; private set; }
        [JsonIgnore]
        public Vector2 C { get; private set; }
        public float Height { get; set; }
        public float Base { get; set; }

        public Triangle()
        {
            
        }

        public Triangle(Vector2 leftUpperCorner, Vector2 vertex, Vector2 leftLowerCorner)
        {
            LeftUpperCorner = leftUpperCorner;
            B = vertex;
            C = leftLowerCorner;
        }
        
        public override Rectangle GetBounds()
        {
            float minX = SMath.Min(LeftUpperCorner.X, SMath.Min(B.X, C.X));
            float minY = SMath.Min(LeftUpperCorner.Y, SMath.Min(B.Y, C.Y));
            float maxX = SMath.Max(LeftUpperCorner.X, SMath.Max(B.X, C.X));
            float maxY = SMath.Max(LeftUpperCorner.Y, SMath.Max(B.Y, C.Y));

            return new Rectangle(new Vector2(minX, minY), new Size( maxX - minX, maxY - minY), 
                Basis);
        }

        public override void Render(DrawingContext drawingContext)
        {
            base.Render(drawingContext);
            DrawLine(drawingContext,
                LeftUpperCorner,
                B,
                GESettings.ColliderPointFillBrush,
                GESettings.ColliderPointPen,
                5, 5, GESettings.ColliderBorderPen);
            DrawLine(drawingContext,
                B,
                C,
                GESettings.ColliderPointFillBrush,
                GESettings.ColliderPointPen,
                5, 5, GESettings.ColliderBorderPen);
            DrawLine(drawingContext,
                C,
                LeftUpperCorner,
                GESettings.ColliderPointFillBrush,
                GESettings.ColliderPointPen,
                5, 5, GESettings.ColliderBorderPen);
        }

        public override List<Vector2> GetNormals()
        {
            List<Vector2> result = new List<Vector2>();
            result.Add(-Basis.X);
            var sideDir = Vector2.Normalize(B - C);
            result.Add(sideDir.GetPerpendicular());
            sideDir = Vector2.Normalize(B - LeftUpperCorner);
            result.Add(sideDir.GetPerpendicular()*-1);
            return result;
        }

        public override void CalculatePoints()
        {
            B = CenterPosition + Basis.X * (2f / 3f * Height * Scale.Width);
            var BaseCenter = CenterPosition + (-Basis.X * (1f / 3f * Height * Scale.Width));
            var Y = (Basis.Y * (Base / 2f) * Scale.Height);
            C = BaseCenter + Y;
            LeftUpperCorner = BaseCenter - Y;
        }

        public override List<Vector2> GetVertexes() =>
            new List<Vector2>() { B, LeftUpperCorner, C };

        public override object Clone()
        {
            return new Triangle() { CenterPosition = CenterPosition, Base = Base, Height = Height };
        }
    }
}
