using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public abstract class UpdatableObject : IUpdatable
    {
        public virtual IGameObjectViewHost GameView => throw new NotImplementedException();

        public virtual IGameTimer GameTimer => throw new NotImplementedException();

        public abstract void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer);

        public abstract void Update();
    }
}
