using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects.Transformable;

namespace WPFGameEngine.WPF.GE.GameObjects.Updatable
{
    public interface IUpdatable : ITransformable
    {
        public bool StartUpCalled { get; }
        public IGameObjectViewHost GameView { get; }

        public IGameTimer GameTimer { get; }

        void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer);

        void Update();
    }
}
