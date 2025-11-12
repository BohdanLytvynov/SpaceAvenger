using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Component.Collider;

namespace WPFGameEngine.Factories.Components.Colliders
{
    public class ColliderFactory : FactoryBase, IColliderFactory
    {
        public ColliderFactory()
        {
            ProductName = nameof(ColliderComponent);
        }
        public override ICollaider Create()
        {
            return new ColliderComponent();
        }
    }
}
