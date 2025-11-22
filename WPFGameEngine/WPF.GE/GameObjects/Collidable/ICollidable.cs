using WPFGameEngine.WPF.GE.Component.Collider;
using WPFGameEngine.WPF.GE.GameObjects.Renderable;

namespace WPFGameEngine.WPF.GE.GameObjects.Collidable
{
    public interface ICollidable : IRenderable
    {
        ICollider Collider { get; }

        public bool IsCollidable { get; }

        void ProcessCollision(List<IGameObject>? collisionInfo);
    }
}
