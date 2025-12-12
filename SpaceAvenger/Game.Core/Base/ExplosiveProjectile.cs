using SpaceAvenger.Game.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class ExplosiveProjectile<TExplosion> : ProjectileBase
        where TExplosion : ExplosionBase
    {
        protected ExplosiveProjectile(Faction faction) : base(faction)
        {
        }

        public override void ProcessCollision(List<IGameObject>? info)
        {
            if (info == null) return;

            var collect = info.Where(x => x is SpaceShipBase s && s.Faction != this.Faction).ToList().Distinct();

            foreach (var obj in collect)
            {
                if (obj is SpaceShipBase s && s.Faction != Faction)
                {
                    s.DoDamage(Damage);
                    Collider.DisableCollision();
                    Hide();
                    var expl = (GameView as IMapableObjectViewHost).Instantiate<TExplosion>();
                    expl.Scale(ExplosionScale);
                    expl.Explode(GetWorldCenter(GetWorldTransformMatrix()));
                }
            }

            //base.ProcessCollision(info);
        }
    }
}
