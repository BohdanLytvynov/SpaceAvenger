using SpaceAvenger.Game.Core.AI;
using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Enums;
using System.Numerics;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace SpaceAvenger.Game.Core.Factions.Neutrals
{
    internal class AstroBase : ExplosiveSpaceShipBase<Explosion3>
    {
        public AstroBase() : base(Faction.Neutrals)
        {
        }

        public override void ConfigureAI(SpaceShipControlModule aIModule)
        {
            //throw new System.NotImplementedException();
        }

        public override void StartUp(IGameObjectViewHost gameObjectViewHost, IGameTimer gameTimer)
        {
            ExplosionSpeed = 0.7f;
            ShipExplosionScale = 7;
            HP = 600f;
            Scale(new Size(0.5, 0.5));
            Translate(new Vector2(40, 40));
            Rotate(90);
            base.StartUp(gameObjectViewHost, gameTimer);
        }

     
    }
}
