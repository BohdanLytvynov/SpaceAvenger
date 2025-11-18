using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Factions.F10.Projectiles;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;

namespace SpaceAvenger.Game.Core.Factions.F10.Weapons
{
    public class F10RailGun : GunBase<F10RailGunProjectile>
    {
        public F10RailGun() : base(nameof(F10RailGun))
        {
            
        }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            ReloadTime = 2.5f;
            ReloadSpeed = 1;
            ShellScaleMultipl = 2.0f / 4.0f;
            base.StartUp(viewHost, gameTimer);
        }
    }
}
