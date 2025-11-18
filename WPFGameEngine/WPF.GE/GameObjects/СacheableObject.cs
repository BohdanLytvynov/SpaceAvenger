using System.Numerics;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public abstract class СacheableObject : MapableObject
    {
        public bool Cached { get; set; }
        public bool UseCaching { get; protected set; }

        public СacheableObject(string name) : base(name)
        {
            UseCaching = true;
            Cached = false;
        }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            base.StartUp(viewHost, gameTimer);
        }

        public override void Update()
        {
            if(!IsInWindowBounds(Transform.Position))
                Disable(true);

            if (!Cached && !IsEnabledAny(this) && UseCaching && GameView is IMapableObjectViewHost movh)
            {
                OnAddToPool();
                movh.ObjectPoolManager.AddToPool(this);
            }

            base.Update();
        }

        public abstract bool IsInWindowBounds(Vector2 position);
        public virtual void OnAddToPool()
        { 
            Cached = false;
        }
        public abstract void OnGetFromPool();

        public void AddToPool(СacheableObject gameObject)
        {
            gameObject.OnAddToPool();
            (GameView as IMapableObjectViewHost).ObjectPoolManager.AddToPool(this);
        }
    }
}
