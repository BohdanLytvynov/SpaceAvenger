using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.WPF.GE.LevelManagers.Base
{
    public class LevelManager : UpdatableBase, ILevelManager
    {
        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            base.StartUp(viewHost, gameTimer);
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
