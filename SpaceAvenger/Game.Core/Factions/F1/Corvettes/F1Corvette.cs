using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base.ShipClasses;
using SpaceAvenger.Game.Core.Enums;
using SpaceAvenger.Game.Core.Factions.F1.Engines;
using SpaceAvenger.Game.Core.Factions.F1.Weapons;
using System.Collections.Generic;
using System.Windows.Media;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;

namespace SpaceAvenger.Game.Core.Factions.F1.Corvettes
{
    public class F1Corvette : CorvetteBase<F1LiteTurret, F1Jet, Explosion3>
    {
        public override List<string> MainEnginesNames => 
            new List<string>() { "Main_Jet" };

        public override List<string> LeftAcceleratorsNames => 
            new List<string>() { "Jet_Accelerator_L_1", "Jet_Accelerator_L_2" };

        public override List<string> RightAcceleratorsNames => 
            new List<string>() { "Jet_Accelerator_R_1", "Jet_Accelerator_R_2" };

        public F1Corvette() : base(Faction.F1)
        {
            HorSpeed = 70f;
            VertSpeed = 30f;
            HP = 600f;
            Shield = 600f;
            ExplosionSpeed = 1f;
            ShipExplosionScale = 4f;
            ShieldRegenSpeed = 10f;
            DetectionDistance = 800f;
        }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            base.StartUp(viewHost, gameTimer);
            TargetMarkerPen = new Pen() { Brush = Brushes.Green };
            TargetMarkerPen.Freeze();
            Disable(true);
        }
    }
}