using SpaceAvenger.Extensions.Math;
using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Factions.F10.Projectiles;
using System.Windows.Media;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Math.Matrixes;

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
            base.StartUp(viewHost, gameTimer);
        }

        public override void Render(DrawingContext dc, Matrix3x3 parent)
        {
            base.Render(dc, parent);

            var Gun_center = GetWorldCenter(GetWorldTransformMatrix());

            var brush = GetLoadIndicatorColor(TimeRemainig / ReloadTime);

            dc.DrawEllipse(brush, new Pen() { Brush = Brushes.Black, Thickness = 0 },
                Gun_center.ToPoint(), Transform.Scale.Width * 7, Transform.Scale.Height * 7);
        }
    }
}
