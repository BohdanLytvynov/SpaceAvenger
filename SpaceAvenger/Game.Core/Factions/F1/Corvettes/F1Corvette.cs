using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Enums;
using SpaceAvenger.Game.Core.Factions.F10.Engines;
using SpaceAvenger.Game.Core.Factions.F10.Weapons;
using SpaceAvenger.Services.WPFInputControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Controllers;

namespace SpaceAvenger.Game.Core.Factions.F1.Corvettes
{
    public class F1Corvette : ExplosiveSpaceShipBase<Explosion3>
    {
        private IEnumerable<F10RailGun> m_battery;
        private Pen m_targetMarkerPen;

        private IEnumerable<F10Jet?> m_mainEngines;
        private IEnumerable<F10Jet?> m_rightAccelerators;
        private IEnumerable<F10Jet?> m_leftAccelerators;

        public F1Corvette() : base(Faction.F1)
        {
            HorSpeed = 70f;
            VertSpeed = 70f;
            HP = 600f;
            Shield = 2000f;
            ShipExplosionScale = 3f;
        }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            base.StartUp(viewHost, gameTimer);
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

        protected override void MoveBackward()
        {
            throw new NotImplementedException();
        }

        protected override void MoveForward()
        {
            throw new NotImplementedException();
        }

        protected override void MoveLeft()
        {
            throw new NotImplementedException();
        }

        protected override void MoveRight()
        {
            throw new NotImplementedException();
        }
    }
}
