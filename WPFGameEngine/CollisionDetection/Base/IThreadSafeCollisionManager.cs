using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.CollisionDetection.Base
{
    public interface IThreadSafeCollisionManager<TCollisionData>
        where TCollisionData : new()
    {
        List<IGameObject> World { get; set; }
        void Start();
        void Pause();
        void Resume();
        void Stop();
        void ForceRemove(int id);
        void Clear();
        List<TCollisionData> GetCollisionInfo(int key);
    }
}
