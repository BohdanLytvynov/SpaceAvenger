using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects.Renderable;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public abstract class MapableObject : RenderableBase
    {
        protected IMapableObjectViewHost? MapableViewHost;

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            MapableViewHost = (viewHost as IMapableObjectViewHost);
            base.StartUp(viewHost, gameTimer);
        }
    }
}
