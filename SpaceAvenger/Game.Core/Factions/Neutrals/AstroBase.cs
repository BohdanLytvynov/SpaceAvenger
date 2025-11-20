using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Enums;
using System.Numerics;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace SpaceAvenger.Game.Core.Factions.Neutrals
{
    internal class AstroBase : SpaceShipBase
    {
        public AstroBase() : base(Faction.Neutrals, nameof(AstroBase))
        {
        }

        public override void StartUp(IGameObjectViewHost gameObjectViewHost, IGameTimer gameTimer)
        {
            HP = 40000f;
            Scale(new Size(0.5, 0.5));
            Translate(new Vector2(40, 40));
            Rotate(90);
            base.StartUp(gameObjectViewHost, gameTimer);
        }
    }
}
