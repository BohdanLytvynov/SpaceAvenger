using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public interface IUpdatable
    {
        public IGameObjectViewHost GameView { get; }

        public IGameTimer GameTimer { get; }

        void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer);

        void Update();
    }
}
