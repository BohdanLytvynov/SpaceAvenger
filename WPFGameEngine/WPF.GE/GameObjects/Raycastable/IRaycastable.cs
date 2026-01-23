using WPFGameEngine.CollisionDetection.CollisionMatrixes;
using WPFGameEngine.CollisionDetection.RaycastManager;
using WPFGameEngine.WPF.GE.Component.Collider;
using WPFGameEngine.WPF.GE.GameObjects.Renderable;

namespace WPFGameEngine.WPF.GE.GameObjects.Raycastable
{
    public interface IRaycastable : IRenderable
    {
        public bool IsRaycastable { get; }
        IRaycastComponent RaycastComponent { get; }
        CollisionLayer CollisionLayer { get; set; }
        void ProcessHit(List<RaycastData> data);
    }
}
