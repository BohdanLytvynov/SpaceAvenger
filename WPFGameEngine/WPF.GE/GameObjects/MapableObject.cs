using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects.Collidable;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public abstract class MapableObject : CollidableBase
    {
        protected IMapableObjectViewHost? MapableViewHost;

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            MapableViewHost = (viewHost as IMapableObjectViewHost);
            base.StartUp(viewHost, gameTimer);
        }
    }
}
