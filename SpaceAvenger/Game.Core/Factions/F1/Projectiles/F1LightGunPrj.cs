using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Enums;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace SpaceAvenger.Game.Core.Factions.F1.Projectiles
{
    public class F1LightGunPrj : ExplosiveProjectile<F1LightPrjExplosion>
    {
        public F1LightGunPrj() : base(Faction.F1)
        {
            ProjectileSpeed = 300f;
            Damage = 200f;
            ExplosionScale = new Size(1f, 1f);
        }
    }
}
