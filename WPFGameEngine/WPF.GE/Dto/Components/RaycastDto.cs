using System.Numerics;
using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Collider;
using WPFGameEngine.WPF.GE.Dto.Base;

namespace WPFGameEngine.WPF.GE.Dto.Components
{
    public class RaycastDto : ComponentDto
    {
        public Vector2 Position { get; set; }

        public override IGEComponent ToObject(IFactoryWrapper factoryWrapper)
        {
            return new RaycastComponent()
            { Position = Position};
        }
    }
}
