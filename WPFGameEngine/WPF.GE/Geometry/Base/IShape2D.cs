using System.Numerics;
using System.Text.Json.Serialization;
using System.Windows.Media;
using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Geometry.Realizations;
using WPFGameEngine.WPF.GE.Math.Basis;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.Geometry.Base
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "RecreateType")]
    [JsonDerivedType(typeof(Circle), nameof(Circle))]
    [JsonDerivedType(typeof(Rectangle), nameof(Rectangle))]
    [JsonDerivedType(typeof(Triangle), nameof(Triangle))]
    public interface IShape2D : IGameEngineEntity
    {
        [JsonIgnore]
        public Size Scale { get; set; }
        [JsonIgnore]
        public Basis2D Basis { get; set; }
        [JsonIgnore]
        public Vector2 CenterPosition { get; set; }
        [JsonIgnore]
        public Vector2 LeftUpperCorner { get; set; }
        void Render(DrawingContext drawingContext);
        bool IntersectsWith(IShape2D other);
        Rectangle GetBounds();
        List<Vector2> GetNormals();
    }
}
