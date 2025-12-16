using System.Numerics;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public abstract class СacheableObject : MapableObject
    {
        public bool Cached { get; set; }
        public bool UseCaching { get; set; }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            UseCaching = true;
            Cached = false;
            base.StartUp(viewHost, gameTimer);
        }

        public override void Update()
        {
            //We cache the objects that extends the Window Bounds and not cached
            if (!IsInWindowBounds(Transform.Position) && !Cached)
            {
                AddToPool(this);
            }
                
            
            base.Update();
        }

        protected virtual bool IsInWindowBounds(Vector2 position) => true;

        public virtual void OnAddToPool()
        {
            Translate(new Vector2(100, 100));
            Disable(true);
            Cached = true;
        }

        public virtual void OnGetFromPool()
        {
            Cached = false;
            Enable(true);
        }

        protected void AddToPool(СacheableObject gameObject)
        {
            if (!UseCaching) return;
            gameObject.OnAddToPool();
            (GameView as IMapableObjectViewHost).ObjectPoolManager.AddToPool(gameObject);
        }
    }
}
