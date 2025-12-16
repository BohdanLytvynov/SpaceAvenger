using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Factions.F10.Projectiles;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;

namespace SpaceAvenger.Game.Core.Factions.F10.Weapons
{
    public class F10RailGun : GunBase<F10RailGunProjectile, GunBlast>
    {
        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            ReloadTime = 2.5f;
            ReloadSpeed = 1;
            ShellScaleMultipl = 2.0f / 4.0f;
            GunBlastScaleMultipl = 1f/2f;
            XAxisGunBlastPositionMultipl = 15f;
            RotationSpeed = 2f;
            AimThreshold = 2.5f;
            base.StartUp(viewHost, gameTimer);
        }
    }
}
