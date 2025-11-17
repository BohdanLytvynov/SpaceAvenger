using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.CollisionDetection.CollisionManager.Base
{
    public interface ICollisionManager
    {
        void AddObject(IGameObject obj);
        void RemoveObject(IGameObject obj);
        void Start();
        void Pause();
        void Resume();
        void Stop();
        void RemoveFromBuffer(int key);
        void Clear();
        List<IGameObject> GetObjects(int key);
    }
}
