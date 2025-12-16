using WPFGameEngine.Factories.Base;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.WPF.GE.AI.Base
{
    public interface IAIModule : IGameEngineEntity
    {
        IGameObjectViewHost GameView { get; }
        void Init(IGameObjectViewHost gameView, IGameObject gameObject);
        void Process(IGameObject gameObject);
    }
}
