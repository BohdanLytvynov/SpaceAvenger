using SpaceAvenger.Extensions.Math;
using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Enums;
using SpaceAvenger.Game.Core.Factions.F10.Engines;
using SpaceAvenger.Game.Core.Factions.F10.Weapons;
using SpaceAvenger.Services.WPFInputControllers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Controllers;
using WPFGameEngine.WPF.GE.Math.Matrixes;

namespace SpaceAvenger.Game.Core.Factions.F10.Destroyer
{
    public class F10Destroyer : ExplosiveSpaceShipBase<Explosion1>
    {
        private IEnumerable<F10RailGun> m_battery;
        private Pen m_targetMarkerPen;

        private IEnumerable<F10Jet?> m_mainEngines;
        private IEnumerable<F10Jet?> m_rightAccelerators;
        private IEnumerable<F10Jet?> m_leftAccelerators;

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
            m_controller = (WPFInputController)GetComponent<ControllerComponent>(false);

            m_battery = GetAllChildrenOfType<F10RailGun>();

            m_Engines = GetAllChildrenOfType<F10Jet>();

            m_mainEngines = m_Engines.Where(e => e.UniqueName.Equals("Jet_Main_L") 
            || e.UniqueName.Equals("Jet_Main_R")).Select(e => e as F10Jet);

            m_leftAccelerators = m_Engines.Where(e => e.UniqueName.Equals("Jet_Accelerator_L_1")
            || e.UniqueName.Equals("Jet_Accelerator_L_2")).Select(e => e as F10Jet);

            m_rightAccelerators = m_Engines.Where(e => e.UniqueName.Equals("Jet_Accelerator_R_1")
            || e.UniqueName.Equals("Jet_Accelerator_R_2")).Select(e => e as F10Jet);
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

        protected override void MoveForward()
        {
            foreach (var item in m_mainEngines)
            {
                item.Start();
            }
        }

        protected override void MoveBackward()
        {
            foreach (var item in m_mainEngines)
            {
                item.Stop();
            }
        }

        protected override void MoveLeft()
        {
            foreach (var item in m_rightAccelerators)
            {
                item.Start();
            }
        }

        protected override void MoveRight()
        {
            foreach (var item in m_leftAccelerators)
            {
                item.Start();
            }
        }
        #endregion
    }
}
