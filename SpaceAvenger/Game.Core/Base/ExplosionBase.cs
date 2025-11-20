using System.Numerics;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class ExplosionBase : СacheableObject
    {
        public ExplosionBase(string name) : base(name)
        {
        }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            Disable(true);
            base.StartUp(viewHost, gameTimer);
        }

        public void Explode(Vector2 position, Size ExplosionSize)
        {
            Enable();
            Scale(ExplosionSize);
            var size = GetActualSize();
            Translate(position - new Vector2(size.Width / 2, size.Height / 2));
            Animation.Start();
        }

        public override void Update()
        {
            if (!Animation.IsRunning && Animation.IsCompleted)
            {
                Disable(true);
                AddToPool(this);
            }

            base.Update();
        }

        public override void OnAddToPool()
        {
            base.OnAddToPool();
        }

        public override void OnGetFromPool()
        {
            Animation.Reset(Animation.Reverse);
            Translate(Vector2.Zero);
        }
    }
}
