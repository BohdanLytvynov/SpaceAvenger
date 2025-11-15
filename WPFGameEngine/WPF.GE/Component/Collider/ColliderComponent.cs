using System.Numerics;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Dto.Base;
using WPFGameEngine.WPF.GE.Dto.Components;
using WPFGameEngine.WPF.GE.Geometry.Base;
using WPFGameEngine.WPF.GE.Math.Basis;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.Component.Collider
{
    [VisibleInEditor(FactoryName = nameof(ColliderComponent),
        DisplayName = "Collider",
        GameObjectType = Enums.GEObjectType.Component)]
    public class ColliderComponent : ComponentBase, ICollider
    {
        public override List<string> IncompatibleComponents => new List<string>() 
        { nameof(ColliderComponent) };
        public IShape2D CollisionShape { get; set; }
        public Size ActualObjectSize { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 ActualCenterPosition
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

        public Basis2D Basis { get; set; }

        public ColliderComponent() : base(nameof(ColliderComponent))
        {
        }

        public override DtoBase ToDto()
        {
            return new ColliderDto()
            { 
                ColliderShape = CollisionShape,
                Position = Position,
            };
        }
    }
}
