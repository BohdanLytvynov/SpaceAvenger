using SpaceAvenger.Extensions.Math;
using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Factions.F10.Projectiles;
using System.Numerics;
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
            XAxisGunBlastPositionMultipl = 20f;
            base.StartUp(viewHost, gameTimer);
        }

        public override void Render(DrawingContext dc, Matrix3x3 parent)
        {
            base.Render(dc, parent);

            var m = GetWorldTransformMatrix();

            var Gun_center = GetWorldCenter(m);

            var brush = GetLoadIndicatorColor(TimeRemainig / ReloadTime);

            dc.DrawEllipse(brush, new Pen() { Brush = Brushes.Black, Thickness = 1f },
                Gun_center.ToPoint(), Transform.Scale.Width * 5f, Transform.Scale.Height * 5f);
        }
    }
}
