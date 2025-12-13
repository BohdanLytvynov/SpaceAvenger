using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Enums;
using SpaceAvenger.Game.Core.Factions.F1.Engines;
using SpaceAvenger.Game.Core.Factions.F1.Weapons;
using System.Collections.Generic;
using System.Windows.Media;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;

namespace SpaceAvenger.Game.Core.Factions.F1.Corvettes
{
    public class F1Corvette : Moveable_Explosive_SpaceShipBase<F1Jet, Explosion3>
    {
        private IEnumerable<F1LiteTurret> m_battery;
        private Pen m_targetMarkerPen;

        public override List<string> MainEnginesNames => 
            new List<string>() { "Main_Jet" };

        public override List<string> LeftAcceleratorsNames => 
            new List<string>() { "Jet_Accelerator_L_1", "Jet_Accelerator_L_2" };

        public override List<string> RightAcceleratorsNames => 
            new List<string>() { "Jet_Accelerator_R_1", "Jet_Accelerator_R_2" };

        public F1Corvette() : base(Faction.F1)
        {
            HorSpeed = 70f;
            VertSpeed = 70f;
            HP = 600f;
            Shield = 1000f;
            ShipExplosionScale = 3f;
        }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            base.StartUp(viewHost, gameTimer);
            m_targetMarkerPen = new Pen();
            m_targetMarkerPen = new Pen() { Brush = Brushes.Green };
            m_targetMarkerPen.Freeze();

            m_battery = GetAllChildrenOfType<F1LiteTurret>();
        }
    }
}