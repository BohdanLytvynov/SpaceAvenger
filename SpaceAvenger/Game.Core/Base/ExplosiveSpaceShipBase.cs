using SpaceAvenger.Game.Core.Enums;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Animations;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class ExplosiveSpaceShipBase<TExplosion> : SpaceShipBase
        where TExplosion : ExplosionBase
    {
        public float ShipExplosionScale { get; set; }
        protected IAnimation ExplosiveAnimation;
        protected float ExplosionSpeed;

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            base.StartUp(viewHost, gameTimer);
        }

        public ExplosiveSpaceShipBase(Faction factionName) : base(factionName)
        {
        }

        public override void Update()
        {
            if (ExplosiveAnimation != null && ExplosiveAnimation.IsRunning && ExplosiveAnimation.CurrentFrameIndex
                == ExplosiveAnimation.AnimationFrames.Count * 1/3)
            {
                base.Destroy();
            }

            base.Update();
        }

        protected override void Destroy()
        {
            var expl = (GameView as IMapableObjectViewHost).Instantiate<TExplosion>();
            ExplosiveAnimation = expl.GetComponent<Animation>();
            ExplosiveAnimation.AnimationSpeed = ExplosionSpeed;
            expl.Scale(Transform.Scale * ShipExplosionScale);
            expl.Explode(GetWorldCenter(GetWorldTransformMatrix()));
        }
    }
}
