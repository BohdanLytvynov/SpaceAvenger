using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Enums;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace SpaceAvenger.Game.Core.Factions.F10.Projectiles
{
    public class F10RailGunProjectile : ExplosiveProjectile<Explosion1>
    {
        public F10RailGunProjectile() : base(Faction.F10)
        {
            ProjectileSpeed = 800f;
            Damage = 100f;
            ExplosionScale = new Size(2,2);
        }
    }
}
