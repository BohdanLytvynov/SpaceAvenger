using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Component.Transforms;

namespace WPFGameEngine.Factories.Components.Transform
{
    public class TransformFactory : FactoryBase, ITransformFactory
    {

        public TransformFactory()
        {
            ProductName = nameof(TransformComponent);
        }

        public override ITransform Create()
        {
            return new TransformComponent();
        }
    }
}
