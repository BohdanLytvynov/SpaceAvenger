using System.Collections.Generic;
using System.Linq;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Base
{
    public enum EngineState
    { 
        Jet_Idle = 0,
        Jet_Move
    }

    public abstract class JetBase : MapableObject
    {
        public IEnumerable<Animator?>? EnginesAnimators { get; private set; }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            EnginesAnimators = GetAllChildrenOfType(GetType().Name)
                .Select(x => x.GetComponent<Animator>());

            base.StartUp(viewHost, gameTimer);
        }


    }
}
