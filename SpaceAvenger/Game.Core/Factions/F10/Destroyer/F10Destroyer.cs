using SpaceAvenger.Extensions.Math;
using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Enums;
using SpaceAvenger.Game.Core.Factions.F10.Engines;
using SpaceAvenger.Game.Core.Factions.F10.Weapons;
using System.Collections.Generic;
using System.Windows.Media;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Math.Matrixes;

namespace SpaceAvenger.Game.Core.Factions.F10.Destroyer
{
    public class F10Destroyer : Moveable_Explosive_SpaceShipBase<F10Jet, Explosion1>
    {
        private IEnumerable<F10RailGun> m_battery;
        private Pen m_targetMarkerPen;

        public F10Destroyer() : base(Faction.F10)
        {
            HorSpeed = 80;
            VertSpeed = 80;
            HP = 4000;
            Shield = 2000;
            ShipExplosionScale = 5;
        }

        #region Methods
        public override void StartUp(IGameObjectViewHost objectViewHost, IGameTimer gameTimer)
        {
            base.StartUp(objectViewHost, gameTimer);
            m_targetMarkerPen = new Pen();
            m_targetMarkerPen = new Pen() { Brush = Brushes.Orange };
            m_targetMarkerPen.Freeze();

            m_battery = GetAllChildrenOfType<F10RailGun>();
        }

        public override void Update()
        {
            if (m_controller != null)
            {
                foreach (var gun in m_battery)
                {
                    gun.LookAt(m_controller.MousePosition, 2, GameTimer.deltaTime.TotalSeconds, gun.GetWorldTransformMatrix());
                }

                if (m_controller.IsMouseButtonDown(System.Windows.Input.MouseButton.Left))
                {
                    foreach (var gun in m_battery)
                    {
                        gun.Shoot(GetDirection(m_controller.MousePosition, GetWorldTransformMatrix()));
                    }
                }
            }

            base.Update();
        }

        public override void Render(DrawingContext dc, Matrix3x3 parent = default)
        {
            base.Render(dc, parent);
            if (m_controller != null)
            {
                foreach (var gun in m_battery)
                {
                    dc.DrawLine(m_targetMarkerPen,
                    gun.GetWorldCenter(gun.GetWorldTransformMatrix()).ToPoint(),
                    m_controller.MousePosition.ToPoint());
                }
            }
        }

        #endregion
    }
}