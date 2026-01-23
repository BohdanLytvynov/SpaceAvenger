using SpaceAvenger.Game.Core.AI;
using SpaceAvenger.Game.Core.Enums;
using SpaceAvenger.Game.Core.UI.Slider;
using SpaceAvenger.Services.WPFInputControllers;
using System.Windows.Media;
using WPFGameEngine.CollisionDetection.CollisionMatrixes;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects.Collidable;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class SpaceShipBase : CollidableBase
    {
        protected WPFInputController m_controller;

        #region Properties
        public CollisionLayer ProjectileCollisionLayer { get; set; }
        public float HP { get; set; }
        public float Shield { get; protected set; }
        public float ShieldRegenSpeed { get; protected set; }
        public float HorSpeed { get; protected set; }
        public float VertSpeed { get; protected set; }
        public float VertAcceleration { get; protected set; }
        public Faction Faction { get; private set; }
        public bool IsAlive { get; private set; }
        public bool IsDestroyed { get; protected set; }
        public float DestrAnimIndex { get; set; }
        protected Bar HPBar;
        protected Bar ShieldBar;

        protected Brush BarLow;
        protected Brush BarHigh;
        protected Brush BarMedium;

        #endregion

        protected SpaceShipBase(Faction factionName)
        {
            Faction = factionName;
        }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            Enable(true);
            IsAlive = true;
            BarLow = Brushes.Red;
            BarHigh = Brushes.Green;
            BarMedium = Brushes.Orange;
            IsDestroyed = false;

            HPBar = FindChild(x => x.UniqueName.Equals("HP")) as Bar;
            HPBar.Max = HP;
            HPBar.Low = BarLow;
            HPBar.Medium = BarMedium;
            HPBar.Full = BarHigh;

            ShieldBar = FindChild(x => x.UniqueName.Equals("Shield")) as Bar;
            if (ShieldBar != null)
            {
                ShieldBar.Max = Shield;
                ShieldBar.Low = BarLow;
                ShieldBar.Medium = BarMedium;
                ShieldBar.Full = BarHigh;
            }

            base.StartUp(viewHost, gameTimer);
        }

        public override void Update()
        {
            var delta = GameTimer.deltaTime;
            float timeDelta = (float)delta.TotalSeconds;

            if (Shield >= ShieldBar.Max)
            {
                Shield = ShieldBar.Max;
            }
            else
            {
                Shield += ShieldRegenSpeed * timeDelta;
            }

            HPBar.Update(HP);
            ShieldBar?.Update(Shield);

            if (HP <= 0 && IsAlive)
            {
                IsAlive = false;
                Destroy();
            }

            base.Update();
        }

        protected virtual void Destroy()
        {
            IsDestroyed = true;
            Disable(true);
            AddToPool(this);
        }

        public override void OnGetFromPool()
        {
            IsDestroyed = false;
            HP = HPBar.Max;
            Shield = ShieldBar?.Max ?? 0;
            IsAlive = true;
            AIModule?.Init(GameView, this);
            base.OnGetFromPool();
            Enable(true);
        }

        public virtual void DoDamage(float damage)
        {
            if (Shield == 0f || Shield - damage <= 0f)
            {
                HP -= damage;
            }
            else
            { 
                Shield -= damage;
            }
        }

        public abstract void ConfigureAI(SpaceShipControlModule aIModule);
    }
}