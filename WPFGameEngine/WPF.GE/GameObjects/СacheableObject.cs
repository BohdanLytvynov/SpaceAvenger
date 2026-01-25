using System.Numerics;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.ObjectPools.ThreadSafePools;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Collider;
using WPFGameEngine.WPF.GE.Settings;

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
            Hide();
            Cached = true;
            Translate(ObjectPoolSettings.ObjectPoolPosition);
        }

        public virtual void OnGetFromPool()
        {
            //ResetRaycastPosition(Transform.Position);
            Cached = false;
        }

        protected void AddToPool(СacheableObject gameObject)
        {
            if (!UseCaching) return;
            if (Cached) return;
            gameObject.OnAddToPool();
            (GameView as IMapableObjectViewHost).ObjectInstantiator.AddToPool(new DelayedItem(gameObject, Delay, GameTimer.totalTime.Milliseconds));
        }
    }
}
