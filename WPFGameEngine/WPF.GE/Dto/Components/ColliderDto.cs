using System.Numerics;
using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Collider;
using WPFGameEngine.WPF.GE.Dto.Base;
using WPFGameEngine.WPF.GE.Geometry.Base;

namespace WPFGameEngine.WPF.GE.Dto.Components
{
    public class ColliderDto : ComponentDto
    {
        public Vector2 Position { get; set; }
        public IShape2D ColliderShape { get; set; }

        public ColliderDto()
        {
            
        }

        public override IGEComponent ToObject(IFactoryWrapper factoryWrapper)
        {
            return new ColliderComponent()
            {
                Position = Position,
                CollisionShape = ColliderShape,
            };
        }
    }
}
