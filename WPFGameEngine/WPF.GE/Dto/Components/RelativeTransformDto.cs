using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;

namespace WPFGameEngine.WPF.GE.Dto.Components
{
    public class RelativeTransformDto : TransformDto
    {
        public bool EnableXAxisCompensation { get; set; }
        public bool EnableYAxisCompensation { get; set; }

        public RelativeTransformDto()
        {
            
        }

        public override IRelativeTransform ToObject(IFactoryWrapper factoryWrapper)
        {
            return new RelativeTransformComponent()
            { 
                Position = Position,
                Rotation = Rotation,
                Scale = Scale,
                CenterPosition = CenterPosition,
                EnableXAxisCompensation = EnableXAxisCompensation,
                EnableYAxisCompensation = EnableYAxisCompensation,
            };
        }
    }
}
