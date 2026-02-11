using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace SpaceAvenger.Game.Core.Factions.F10.Projectiles
{
    public class F10RailGunProjectile : ExplosiveProjectile<Explosion1>
    {
        public F10RailGunProjectile() 
        {
            ProjectileSpeed = 800f;
            Damage = 100f;
            ExplosionScale = new Size(1f,1f);
        }
    }
}
