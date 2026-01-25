using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Component.Collider.Base;
using WPFGameEngine.WPF.GE.Dto.Base;
using WPFGameEngine.WPF.GE.Dto.Components;
using WPFGameEngine.WPF.GE.Geometry.Base;

namespace WPFGameEngine.WPF.GE.Component.Collider
{
    [VisibleInEditor(FactoryName = nameof(ColliderComponent),
        DisplayName = "Collider Component",
        GameObjectType = Enums.GEObjectType.Component)]
    public class ColliderComponent : ColliderComponentBase, ICollider
    {
        public override List<string> IncompatibleComponents => new List<string>() 
        { nameof(ColliderComponent), nameof(RaycastComponent) };
        public IShape2D CollisionShape { get; set; }

        public ColliderComponent() : base(nameof(ColliderComponent))
        {
        }

        public override DtoBase ToDto()
        {
            return new ColliderDto()
            { 
                ColliderShape = CollisionShape!,
                Position = Position,
            };
        }

        #region IClonable

        public override object Clone()
        {
            return new ColliderComponent()
            { 
                CollisionShape = (IShape2D)CollisionShape.Clone(),
                Position = Position,
            };
        }

        #endregion
    }
}
