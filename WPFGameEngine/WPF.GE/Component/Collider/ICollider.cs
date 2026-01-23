using WPFGameEngine.WPF.GE.Component.Collider.Base;
using WPFGameEngine.WPF.GE.Geometry.Base;

namespace WPFGameEngine.WPF.GE.Component.Collider
{
    public interface ICollider : IColliderComponentBase
    {
        /// <summary>
        /// Shape of the collider
        /// </summary>
        IShape2D CollisionShape { get; set; }
    }
}
