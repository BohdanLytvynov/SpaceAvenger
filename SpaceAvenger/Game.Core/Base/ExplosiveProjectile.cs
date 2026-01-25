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
        private object m_hitLock;
        private List<RaycastData> m_raycastInfo;

        protected ExplosiveProjectile(Faction faction) : base(faction)
        {
            m_hitLock = new object();
            m_raycastInfo = new List<RaycastData>();
        }

        public override void ProcessHit(List<RaycastData>? info)
        {
            if (info == null) return;

            lock (m_hitLock)
            {
                m_raycastInfo.Clear();
                m_raycastInfo.AddRange(info);
            }

            foreach (var obj in m_raycastInfo)
            {
                if (obj.Object is SpaceShipBase s)
                {
                    s.DoDamage(Damage);
                    ColliderComponent.DisableCollision();
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
            ColliderComponent.EnableCollision();
            base.OnGetFromPool();
        }
    }
}
