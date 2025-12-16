using WPFGameEngine.GameViewControl;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.WPF.GE.AI.Base
{
    public abstract class AIModuleBase : IAIModule
    {
        public IGameObjectViewHost GameView { get; private set; }
        protected AIModuleBase()
        {

        }
        public virtual void Process(IGameObject gameObject)
        { 
            if(!gameObject.Enabled) return;
        }

        public virtual void Init(IGameObjectViewHost gameView, IGameObject gameObject)
        {
            GameView = gameView ?? throw new ArgumentNullException(nameof(gameView));
        }
    }
}
