using SpaceAvenger.Game.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using WPFGameEngine.Extensions;
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
                if (obj is SpaceShipBase s && s.Faction != this.Faction)
                {
                    s.DoDamage(Damage);
                    Collider.DisableCollision();
                    var matrix = GetWorldTransformMatrix();
                    var prevPos = GetWorldCenter(matrix);
                    AddToPool(this);
                    var expl = (GameView as IMapableObjectViewHost).Instantiate<TExplosion>();
                    var angle = matrix.GetBasis().X.GetAngleDeg(expl.GetBasis().X);
                    expl.Rotate(angle);
                    expl.Scale(ExplosionScale);
                    expl.Explode(prevPos);
                }
            }
        }

        public override void OnGetFromPool()
        {
            Collider.EnableCollision();
            base.OnGetFromPool();
        }
    }
}
