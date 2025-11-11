using System.Numerics;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public abstract class СacheableObject : MapableObject
    {  
        public bool UseCaching { get; protected set; }
        public СacheableObject(string name) : base(name)
        {
            UseCaching = true;
        }

        public override void StartUp()
        {
            Enable();
            base.StartUp();
        }

        public override void Update(IGameObjectViewHost world, IGameTimer gameTimer)
        {
            if(!IsInWindowBounds(Transform.Position))
                Disable();

            if (!Enabled && UseCaching && world is IMapableObjectViewHost movh)
            {
                ResetState();
                movh.ObjectPoolManager.AddToPool(this);
            }

            base.Update(world, gameTimer);
        }

        public abstract bool IsInWindowBounds(Vector2 position);

        public virtual void ResetState()
        {
            m_init = false;//For using StartUp Again
        }
    }
}
