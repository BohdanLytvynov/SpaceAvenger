using System.Numerics;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.ObjectPools.ThreadSafePools;
using WPFGameEngine.Timers.Base;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public abstract class СacheableObject : MapableObject
    {
        public bool Cached { get; set; }
        public bool UseCaching { get; set; }
        /// <summary>
        /// Controls Time of Delay, that must pass before object will be ready to use from the Object Pool, Set in ms
        /// </summary>
        public double Delay { get; protected set; }

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
            Translate(Vector2.Zero);
            Hide();
            Cached = true;
        }

        public virtual void OnGetFromPool()
        {
            Cached = false;
        }

        protected void AddToPool(СacheableObject gameObject)
        {
            if (!UseCaching) return;
            if (Cached) return;
            (GameView as IMapableObjectViewHost).ObjectInstantiator.AddToPool(new DelayedItem(gameObject, Delay));
        }
    }
}
