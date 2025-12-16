using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base.ShipClasses;
using SpaceAvenger.Game.Core.Enums;
using SpaceAvenger.Game.Core.Factions.F10.Engines;
using SpaceAvenger.Game.Core.Factions.F10.Weapons;
using System.Collections.Generic;
using System.Windows.Media;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;

namespace SpaceAvenger.Game.Core.Factions.F10.Destroyer
{
    public class F10Destroyer : DestroyerBase<F10RailGun, F10Jet, Explosion1>
    {
        public override List<string> MainEnginesNames => 
            new List<string>() { "Jet_Main_L", "Jet_Main_R" };

        public override List<string> LeftAcceleratorsNames => 
            new List<string>() { "Jet_Accelerator_L_1", "Jet_Accelerator_L_2" };

        public override List<string> RightAcceleratorsNames => 
            new List<string>() { "Jet_Accelerator_R_1", "Jet_Accelerator_R_2" };

        public F10Destroyer() : base(Faction.F10)
        {
            HorSpeed = 80;
            VertSpeed = 80;
            HP = 4000;
            Shield = 2000;
            ShipExplosionScale = 5;
            ShipExplosionScale = 5f;
            ShieldRegenSpeed = 20f;
        }

        #region Methods
        public override void StartUp(IGameObjectViewHost objectViewHost, IGameTimer gameTimer)
        {
            base.StartUp(objectViewHost, gameTimer);
            TargetMarkerPen = new Pen() { Brush = Brushes.Orange };
            TargetMarkerPen.Freeze();
        }

        public override void Update()
        {
            if (m_controller != null)
            {
                AimWeapons(m_controller.MousePosition);

                if (m_controller.IsMouseButtonDown(System.Windows.Input.MouseButton.Left))
                {
                    ShootWeapons(m_controller.MousePosition);
                }
            }

            base.Update();
        }
        #endregion
    }
}