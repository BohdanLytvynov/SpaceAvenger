using SpaceAvenger.Game.Core.Base;

namespace SpaceAvenger.Game.Core.Factions.Neutrals
{
    internal class AstroBase : SpaceShipBase
    {
        public AstroBase() : base(nameof(AstroBase))
        {
            HP = 100f;
        }
    }
}
