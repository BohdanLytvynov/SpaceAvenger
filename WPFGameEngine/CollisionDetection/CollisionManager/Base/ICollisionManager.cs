using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.CollisionDetection.CollisionManager.Base
{
    public interface ICollisionManager
    {
        List<IGameObject> World { get; set; }
        void Start();
        void Pause();
        void Resume();
        void Stop();
        void ForceRemove(int id);
        void Clear();
        List<CollisionData>? GetCollisionInfo(int key);
    }
}