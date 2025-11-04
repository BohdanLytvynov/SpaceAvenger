using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;

namespace WPFGameEngine.Factories.Components.RelativeTransforms
{
    public class RelativeTransformFactory : FactoryBase, IRelativeTransformFactory
    {
        public RelativeTransformFactory()
        {
            ProductName = nameof(RelativeTransformComponent);
        }

        public override IRelativeTransform Create()
        {
            return new RelativeTransformComponent();
        }
    }
}
