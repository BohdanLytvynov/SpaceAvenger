using System.Numerics;
using System.Text.Json.Serialization;
using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Dto.Base;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.Dto.Components
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "RecreateType")]
    [JsonDerivedType(typeof(RelativeTransformDto), nameof(RelativeTransformComponent))]
    public class TransformDto : ComponentDto
    {
        public Vector2 Position { get; set; }
        public Vector2 CenterPosition { get; set; }
        public double Rotation { get; set; }//Degree
        public Size Scale { get; set; }

        public TransformDto()
        {
            
        }

        public override ITransform ToObject(IFactoryWrapper factoryWrapper)
        {
            return new TransformComponent(Position, CenterPosition, Rotation, Scale);
        }
    }
}
