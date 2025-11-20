using SpaceAvenger.Game.Core.Enums;
using System.Numerics;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class ProjectileBase : СacheableObject
    {
        private Vector2 m_dir;
        public float ProjectileSpeed { get; protected set; }
        public bool Move { get; protected set; }
        public float Damage { get; protected set; }
        public Faction Faction { get; private set; }
        public Size ExplosionScale { get; protected set; }

        protected ProjectileBase(Faction faction, string name) : base(name)
        {
            Faction = faction;
        }

        public override void StartUp(IGameObjectViewHost gameObjectViewHost, IGameTimer gameTimer)
        {
            Move = false;
            base.StartUp(gameObjectViewHost, gameTimer);
        }

        protected override bool IsInWindowBounds(Vector2 position)
        {
            var w = App.Current.MainWindow;
            double winWidth = w.Width;
            double winHeight = w.Height;
            float x = position.X;
            float y = position.Y;
            return x >= 0 && x <= winWidth && y >= 0 && y <=winHeight;
        }

        public override void Update()
        {
            if(Move)
                Translate(m_dir, ProjectileSpeed, GameTimer.deltaTime.TotalSeconds);

            base.Update();
        }

        public virtual void Fire(Vector2 dir)
        {
            m_dir = dir;
            Move = true;
            Enable();
        }

        public override void OnAddToPool()
        {
            m_dir = Vector2.Zero;
            Move = false;
            base.OnAddToPool();
        }

        public override void OnGetFromPool()
        {
            Collider.EnableCollision();
            base.OnGetFromPool();
        }
    }
}
