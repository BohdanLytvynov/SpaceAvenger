using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Enums;

namespace SpaceAvenger.Game.Core.Factions.F1.Projectiles
{
    public class F1LightGunPrj : ExplosiveProjectile<F1LightPrjExplosion>
    {
        public F1LightGunPrj() : base(Faction.F1)
        {
        }
    }
}
