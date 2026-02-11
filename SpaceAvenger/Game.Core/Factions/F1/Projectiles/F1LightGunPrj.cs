using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace SpaceAvenger.Game.Core.Factions.F1.Projectiles
{
    public class F1LightGunPrj : ExplosiveProjectile<F1LightPrjExplosion>
    {
        public F1LightGunPrj()
        {
            ProjectileSpeed = 300f;
            Damage = 50;
            ExplosionScale = new Size(1f, 1f);
        }
    }
}
