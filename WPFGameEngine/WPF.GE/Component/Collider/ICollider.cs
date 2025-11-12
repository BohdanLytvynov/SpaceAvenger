using System.Numerics;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Geometry.Base;

namespace WPFGameEngine.WPF.GE.Component.Collider
{
    public interface ICollider : IGEComponent
    {
        IShape2D CollisionShape { get; set; }
    }
}
