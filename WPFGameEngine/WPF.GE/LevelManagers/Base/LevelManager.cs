using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;

namespace WPFGameEngine.WPF.GE.LevelManagers.Base
{
    public class LevelManager : ILevelManager
    {
        public IGameObjectViewHost GameView { get; private set; }

        public IGameTimer GameTimer { get; private set; }
        public double ZIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Id => throw new NotImplementedException();

        public void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            GameTimer = gameTimer;
            GameView = viewHost;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
