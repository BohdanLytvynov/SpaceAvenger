using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Attributes.Factories;
using WPFGameEngine.Enums;
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
        public Vector2 Vertex { get; private set; }
        public Vector2 LeftLowerCorner { get; private set; }
        public Vector2 BaseCenter { get; set; }
        public float Height { get; set; }
        public float Base { get; set; }

        public Triangle()
        {
            
        }

        public Triangle(Vector2 leftUpperCorner, Vector2 vertex, Vector2 leftLowerCorner)
        {
            LeftUpperCorner = leftUpperCorner;
            Vertex = vertex;
            LeftLowerCorner = leftLowerCorner;
        }
        
        public override Rectangle GetBounds()
        {
            float minX = SMath.Min(LeftUpperCorner.X, SMath.Min(Vertex.X, LeftLowerCorner.X));
            float minY = SMath.Min(LeftUpperCorner.Y, SMath.Min(Vertex.Y, LeftLowerCorner.Y));
            float maxX = SMath.Max(LeftUpperCorner.X, SMath.Max(Vertex.X, LeftLowerCorner.X));
            float maxY = SMath.Max(LeftUpperCorner.Y, SMath.Max(Vertex.Y, LeftLowerCorner.Y));

            return new Rectangle(new Vector2(minX, minY), new Size( maxX - minX, maxY - minY), 
                Basis);
        }

        public override void Render(DrawingContext drawingContext)
        {
            base.Render(drawingContext);

            drawingContext.DrawEllipse(Brushes.Red, new Pen(),
                   new System.Windows.Point(LeftUpperCorner.X, LeftUpperCorner.Y),
                   5, 5);

            drawingContext.DrawEllipse(Brushes.Green, new Pen(),
                   new System.Windows.Point(Vertex.X, Vertex.Y),
                   5, 5);

            drawingContext.DrawEllipse(Brushes.Blue, new Pen(),
                   new System.Windows.Point(BaseCenter.X, BaseCenter.Y),
                   5, 5);

            drawingContext.DrawEllipse(Brushes.Pink, new Pen(),
                   new System.Windows.Point(CenterPosition.X, CenterPosition.Y),
                   5, 5);

            //DrawLine(drawingContext,
            //    LeftUpperCorner,
            //    Vertex,
            //    GESettings.ColliderPointFillBrush,
            //    GESettings.ColliderPointPen,
            //    5, 5, GESettings.ColliderBorderPen);
            //DrawLine(drawingContext,
            //    Vertex,
            //    LeftLowerCorner,
            //    GESettings.ColliderPointFillBrush,
            //    GESettings.ColliderPointPen,
            //    5, 5, GESettings.ColliderBorderPen);
            //DrawLine(drawingContext,
            //    LeftLowerCorner,
            //    LeftUpperCorner,
            //    GESettings.ColliderPointFillBrush,
            //    GESettings.ColliderPointPen,
            //    5, 5, GESettings.ColliderBorderPen);
        }

        public override List<Vector2> GetNormals()
        {
            List<Vector2> result = new List<Vector2>();



            return result;
        }

        protected override void CalculatePoints()
        {
            Vertex = CenterPosition + Basis.X * (2f / 3f * Height * Scale.Width);
            BaseCenter = CenterPosition + (-Basis.X * (1f / 3f * Height * Scale.Height));
            //LeftLowerCorner = CenterPosition + (Basis.Y * (Base / 2 * Scale.Width)) + baseCenter;
            LeftUpperCorner = CenterPosition - ((-Basis.Y * (Base / 2f * Scale.Width)) + BaseCenter);
        }
    }
}
