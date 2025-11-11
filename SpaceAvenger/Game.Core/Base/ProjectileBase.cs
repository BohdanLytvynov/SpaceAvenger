using System.Numerics;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class ProjectileBase : СacheableObject
    {
        private Vector2 m_dir;
        public float ProjectyleSpeed { get; protected set; }
        public bool Fired { get; protected set; }
        

        protected ProjectileBase(string name) : base(name)
        {
        }

        public override void StartUp()
        {
            Fired = false;
            base.StartUp();
        }

        public override bool IsInWindowBounds(Vector2 position)
        {
            var w = App.Current.MainWindow;
            
            double winWidth = w.Width;
            double winHeight = w.Height;
            float x = position.X;
            float y = position.Y;
            return x >= 0 && x <= winWidth && y >= 0 && y <=winHeight;
        }

        public override void Update(IGameObjectViewHost world, IGameTimer gameTimer)
        {
            if (!Fired)
                return;

            Translate(m_dir, ProjectyleSpeed, gameTimer.deltaTime.TotalSeconds);
            base.Update(world, gameTimer);
        }

        public virtual void Fire(Vector2 dir)
        {
            m_dir = dir;
            Fired = true;
        }

        public override void ResetState()
        {
            m_dir = Vector2.Zero;
            Fired = false;
            base.ResetState();
        }
    }
}
