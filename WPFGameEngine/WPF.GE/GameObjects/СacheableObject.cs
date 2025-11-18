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

        protected virtual bool IsInWindowBounds(Vector2 position) => true;

        protected virtual void OnAddToPool()
        { 
            
        }
        public virtual void OnGetFromPool()
        {
            Cached = false;
        }

        protected void AddToPool(СacheableObject gameObject)
        {
            gameObject.OnAddToPool();
            gameObject.Cached = true;
            (GameView as IMapableObjectViewHost).ObjectPoolManager.AddToPool(this);
        }
    }
}
