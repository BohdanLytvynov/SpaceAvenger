using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Factions.F1.Projectiles;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;

namespace SpaceAvenger.Game.Core.Factions.F1.Weapons
{
    public class F1LiteTurret : GunBase<F1LightGunPrj, F1LightGunBlast>
    {
        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            ReloadTime = 0.15f;
            ReloadSpeed = 0.05f;
            ShellScaleMultipl = 2.0f / 4.0f;
            GunBlastScaleMultipl = 1f / 2f;
            XAxisGunBlastPositionMultipl = 0f;
            RotationSpeed = 5f;
            AimThreshold = 2f;
            base.StartUp(viewHost, gameTimer);
        }
    }
}
