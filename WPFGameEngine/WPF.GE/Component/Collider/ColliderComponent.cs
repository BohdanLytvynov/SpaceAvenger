using System.Numerics;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Component.Collider.Base;
using WPFGameEngine.WPF.GE.Dto.Base;
using WPFGameEngine.WPF.GE.Dto.Components;
using WPFGameEngine.WPF.GE.Geometry.Base;

namespace WPFGameEngine.WPF.GE.Component.Collider
{
    [VisibleInEditor(FactoryName = nameof(ColliderComponent),
        DisplayName = "Collider",
        GameObjectType = Enums.GEObjectType.Component)]
    public class ColliderComponent : ColliderComponentBase, ICollider
    {
        public override Vector2 ActualCenterPosition
        {
            get
            {
                var actX = Position.X * ActualObjectSize.Width;
                var actY = Position.Y * ActualObjectSize.Height;
                var locX = Basis.X * actX;
                var locY = Basis.Y * actY;
                return locX + locY;
            }
        }
        public override List<string> IncompatibleComponents => new List<string>() 
        { nameof(ColliderComponent), nameof(RaycastComponent) };
        public IShape2D CollisionShape { get; set; }

        public ColliderComponent() : base(nameof(ColliderComponent))
        {
            EnableCollision();
            ResolveCollision();
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
