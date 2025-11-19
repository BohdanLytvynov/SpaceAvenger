using SpaceAvenger.Game.Core.Base;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;

namespace SpaceAvenger.Game.Core.Animations.Explosions
{
    public class Explosion1 : ExplosionBase
    {
        public Explosion1() : base(nameof(Explosion1))
        {

        }
        
        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            Disable();
            Animation.Stop();
            Animation.Reset(Animation.Reverse);
            base.StartUp(viewHost, gameTimer);
        }
    }
}
