using SpaceAvenger.Game.Core.Enums;
using System.Collections.Generic;
using WPFGameEngine.CollisionDetection.RaycastManager;
using WPFGameEngine.Extensions;
using WPFGameEngine.GameViewControl;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class ExplosiveProjectile<TExplosion> : ProjectileBase
        where TExplosion : ExplosionBase
    {
        protected ExplosiveProjectile(Faction faction) : base(faction)
        {
        }

        public override void ProcessHit(List<RaycastData>? info)
        {
            if (info == null) return;
            
            foreach (var obj in info)
            {
                if (obj.Object is SpaceShipBase s)
                {
                    s.DoDamage(Damage);
                    RaycastComponent.DisableCollision();
                    base.ProcessHit(info);

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
            RaycastComponent.EnableCollision();
            base.OnGetFromPool();
        }
    }
}
