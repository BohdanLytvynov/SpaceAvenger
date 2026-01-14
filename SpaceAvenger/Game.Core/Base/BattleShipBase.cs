using SpaceAvenger.Extensions.Math;
using SpaceAvenger.Game.Core.Base.Interfaces;
using SpaceAvenger.Game.Core.Enums;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.CollisionDetection.CollisionMatrixes;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Math.Matrixes;

namespace SpaceAvenger.Game.Core.Base
{
    public enum WeaponType
    {
        Primary = 0,
    }

    public abstract class BattleShipBase<TPrimWeapons, TJet, TExplosion>
        : Moveable_Explosive_SpaceShipBase<TJet, TExplosion>, IBattleShip
        where TPrimWeapons : WeaponBase
        where TJet : JetBase
        where TExplosion : ExplosionBase
    {
        private bool m_useThreshold;
        public Pen TargetMarkerPen { get; protected set; }
        private IEnumerable<TPrimWeapons> m_PrimWeapons;
        public WeaponType WeaponType { get; protected set; }
        public float DetectionDistance { get; set; }
        public bool WeaponsAimed 
        { 
            get
            {
                var weapons = GetActiveWeapons();

                foreach (var item in weapons)
                {
                    if(!item.WeaponAimed) 
                        return false;
                }

                return true;
            }
        }

        private Vector2 m_targetPosition;

        protected BattleShipBase(Faction faction) : base(faction)
        {
        }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            base.StartUp(viewHost, gameTimer);
            m_PrimWeapons = GetAllChildrenOfType<TPrimWeapons>();

            SetProjectileCollisionLayer(m_PrimWeapons, ProjectileCollisionLayer);
        }

        public override void Update()
        {
            var delta = GameTimer.deltaTime;
            float timeDelta = (float)delta.TotalSeconds;

            var weapons = GetActiveWeapons();

            foreach (WeaponBase item in weapons)
            {
                var wm = item.GetWorldTransformMatrix();

                if(!m_useThreshold)
                    item.LookAt(m_targetPosition, item.RotationSpeed, timeDelta,
                        wm);
                else
                    item.WeaponAimed = item.LookAtWithTreshold(m_targetPosition, item.RotationSpeed,
                        timeDelta, wm, item.AimThreshold);
            }

            base.Update();
        }

        public override void Render(DrawingContext dc, Matrix3x3 parent = default)
        {
            base.Render(dc, parent);
            if (m_controller != null)
            {
                var weapons = GetActiveWeapons();

                foreach (var gun in weapons)
                {
                    dc.DrawLine(TargetMarkerPen,
                    gun.GetWorldCenter(gun.GetWorldTransformMatrix()).ToPoint(),
                    m_controller.MousePosition.ToPoint());
                }
            }
        }

        public void AimWeapons(Vector2 position, bool useThreshold = false)
        {
            m_targetPosition = position;
            m_useThreshold = useThreshold;
        }

        public void SetWeaponType(WeaponType weaponType)
        {
            WeaponType = weaponType;
        }

        public void ShootWeapons(Vector2 targetDir)
        {
            var weapons = GetActiveWeapons();

            foreach (var gun in weapons)
            {
                gun.Shoot(GetDirection(targetDir,
                    GetWorldTransformMatrix()));
            }
        }

        public virtual IEnumerable<WeaponBase>? GetActiveWeapons()
        {
            switch (WeaponType)
            {
                case WeaponType.Primary:
                    return m_PrimWeapons;
            }

            return null;
        }

        private void SetProjectileCollisionLayer(IEnumerable<WeaponBase> weapons, CollisionLayer collisionLayer)
        {
            foreach (var w in weapons)
            {
                w.ProjectilesCollisionLayer = collisionLayer;
            }
        }
    }
}
