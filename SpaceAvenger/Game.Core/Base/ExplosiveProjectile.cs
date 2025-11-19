using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using WPFGameEngine.GameViewControl;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class ExplosiveProjectile<TExplosion> : ProjectileBase
        where TExplosion : ExplosionBase
    {
        protected ExplosiveProjectile(Faction faction, string name) : base(faction, name)
        {
        }

        public override void ProcessCollision(CollisionInfo? info)
        {
            if (info == null) return;

            foreach (var obj in info.ObjectsWithCollision)
            {
                if (obj is SpaceShipBase s && s.Faction != Faction)
                {
                    Move = false;
                    s.HP -= Damage;
                    Disable();
                    var expl = (GameView as IMapableObjectViewHost).Instantiate<TExplosion>();
                    expl.Explode(GetWorldCenter(), ExplosionScale);
                }
            }

            base.ProcessCollision(info);
        }
    }
}
