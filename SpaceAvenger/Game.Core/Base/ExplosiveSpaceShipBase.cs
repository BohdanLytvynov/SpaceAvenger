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

        public ExplosiveSpaceShipBase(Faction factionName) : base(factionName)
        {
            DestrAnimIndex = 1f / 3f;
        }

        public override void Update()
        {
            if (ExplosiveAnimation != null 
                && ExplosiveAnimation.IsRunning && (ExplosiveAnimation.CurrentFrameIndex
                == (int)(ExplosiveAnimation.AnimationFrames.Count * DestrAnimIndex))               )
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
