using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using WPFGameEngine.Enums;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.GameViewControl
{
    public interface IGameObjectViewHost
    {
        GameState GameState { get; }
        void AddObject(IGameObject gameObject);
        void AddObjects(IEnumerable<IGameObject> gameObjects);
        public void RemoveObject(IGameObject gameObject);
        public void StartGame();
        public void Resume();
        public void Pause();
        public void Stop();
        public void ClearWorld();
    }
}
