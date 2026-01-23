using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using WPFGameEngine.CollisionDetection.RaycastManager;

namespace WPFGameEngine.GameViewControl
{
    public interface IColliderView
    {
        ICollisionManager CollisionManager { get; }
        IRaycastManager RaycastManager { get; }
    }
}
