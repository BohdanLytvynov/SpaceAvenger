using System.Numerics;
using System.Text.Json.Serialization;
using System.Windows.Media;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;
using WPFGameEngine.WPF.GE.Geometry.Realizations;
using WPFGameEngine.WPF.GE.Helpers;
using WPFGameEngine.WPF.GE.Math.Basis;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.Geometry.Base
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "RecreateType")]
    [JsonDerivedType(typeof(Circle), nameof(Circle))]
    public abstract class Shape2D : IShape2D
    {
        public Shape2D()
        {
            
        }

        public Vector2 CenterPosition { get; set; }
        public Vector2 LeftUpperCorner { get; set; }
        public Basis2D Basis { get; set; }
        public Size Scale { get; set; }

        //public abstract Rectangle GetBounds();
        public bool IntersectsWith(IShape2D other)
        { 
            return CollisionHelper.Intersects(this, other);
        }
        public abstract void Render(DrawingContext drawingContext);
    }
}
