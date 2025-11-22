using System.Numerics;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class ExplosionBase : СacheableObject
    {
        private Size m_ExplosionSize;

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            Disable(true);
            Animation.Stop();
            Animation.Reset(Animation.Reverse);
            base.StartUp(viewHost, gameTimer);
        }

        public void Explode(Vector2 position)
        {
            Enable();
            m_ExplosionSize = GetActualSize();
            Translate(position - new Vector2(m_ExplosionSize.Width / 2, m_ExplosionSize.Height / 2));//Move to the center origin of the texture
            Animation.Start();
        }

        public void UpdatePosition(Vector2 position)
        {
            Translate(position - new Vector2(m_ExplosionSize.Width / 2, m_ExplosionSize.Height / 2));//Move to the center origin of the texture
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
