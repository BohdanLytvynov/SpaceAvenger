using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using WPFGameEngine.CollisionDetection.CollisionMatrixes;
using WPFGameEngine.WPF.GE.Component.Collider;
using WPFGameEngine.WPF.GE.GameObjects.Renderable;

namespace WPFGameEngine.WPF.GE.GameObjects.Collidable
{
    public interface ICollidable : IRenderable
    {
        CollisionLayer CollisionLayer { get; set; }
        ICollider Collider { get; }
        public bool IsCollidable { get; }
        void ProcessCollision(List<CollisionData>? collisionInfo);
    }
}
