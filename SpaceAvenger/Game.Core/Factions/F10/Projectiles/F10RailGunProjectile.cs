using SpaceAvenger.Game.Core.Base;

namespace SpaceAvenger.Game.Core.Factions.F10.Projectiles
{
    public class F10RailGunProjectile : ProjectileBase
    {
        public F10RailGunProjectile() : base(nameof(F10RailGunProjectile))
        {
            ProjectyleSpeed = 800f;
        }
    }
}
