using WPFGameEngine.Enums;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.GameViewControl
{
    public interface IGameObjectViewHost
    {
        IGameTimer GameTimer { get; }
        GameState GameState { get;}
        void AddObject(IGameObject gameObject, Action<IGameObject> preStartUpConfig = null,
            Action<IGameObject> postStartUpConfig = null);
        public void RemoveObject(IGameObject gameObject);
        public IGameObject GetObject(Func<IGameObject, bool> predicate, bool recursiveSearch = false);
        public void StartGame();
        public void Resume();
        public void Pause();
        public void Stop();
        public void ClearWorld();
    }
}
