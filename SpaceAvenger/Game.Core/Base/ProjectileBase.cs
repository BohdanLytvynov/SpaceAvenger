using SpaceAvenger.Game.Core.Enums;
using System.Numerics;
using WPFGameEngine.CollisionDetection.Grid;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Collider;
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

        protected ProjectileBase(Faction faction)
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
            double winWidth = w.ActualWidth;
            double winHeight = w.ActualHeight;
            float x = position.X;
            float y = position.Y;
            return (x >= 0 && x <= winWidth) && (y >= 0 && y <=winHeight);
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
        }

        public override void OnAddToPool()
        {
            Move = false;
            m_dir = Vector2.Zero;
            base.OnAddToPool();
        }

        public override void Translate(Vector2 position)
        {
            ResetRaycastPosition(position);
            base.Translate(position);
        }
    }
}
