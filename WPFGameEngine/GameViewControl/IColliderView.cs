using WPFGameEngine.CollisionDetection.CollisionManager.Base;

namespace WPFGameEngine.GameViewControl
{
    public interface IColliderView
    {
        ICollisionManager CollisionManager { get; }
    }
}
