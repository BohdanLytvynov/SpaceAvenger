using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Dto.Base;
using WPFGameEngine.WPF.GE.Geometry.Base;

namespace WPFGameEngine.WPF.GE.Component.Collider
{
    [VisibleInEditor(FactoryName = nameof(ColliderComponent),
        DisplayName = "Collider",
        GameObjectType = Enums.GEObjectType.Component)]
    public class ColliderComponent : ComponentBase, ICollider
    {
        public ColliderComponent() : base(nameof(ColliderComponent))
        {
        }

        public override List<string> IncompatibleComponents => new List<string>();

        public IShape2D CollisionShape { get; set; }

        public override DtoBase ToDto()
        {
            
            throw new NotImplementedException();
        }
    }
}
