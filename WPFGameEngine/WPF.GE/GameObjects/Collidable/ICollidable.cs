using System.Numerics;
using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using WPFGameEngine.CollisionDetection.CollisionMatrixes;
using WPFGameEngine.CollisionDetection.RaycastManager;
using WPFGameEngine.WPF.GE.Component.Collider.Base;
using WPFGameEngine.WPF.GE.GameObjects.Renderable;

namespace WPFGameEngine.WPF.GE.GameObjects.Collidable
{
    public interface ICollidable : IRenderable
    {
        IColliderComponentBase ColliderComponent { get; }
        CollisionLayer CollisionLayer { get; set; }
        bool IsCollidable { get; }
        bool IsRaycastable { get; }
        void ProcessCollision(List<CollisionData>? collisionInfo);
        void ProcessHit(List<RaycastData> data);
        void ResetRaycastPosition(Vector2 newPosition);
    }
}
