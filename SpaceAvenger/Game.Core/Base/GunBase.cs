using SpaceAvenger.Game.Core.Factions.F10.Projectiles;
using System.Drawing;
using System.Numerics;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Base
{
    public class GunBase<TShell> : MapableObject
        where TShell : ProjectileBase
    {
        public float TimeRemainig { get; private set; }
        //Seconds
        public float ReloadSpeed { get; protected set; }
        public float ReloadTime { get; protected set; }
        public bool GunLoaded { get => TimeRemainig <= 0; }
        public float ShellScaleMultipl { get; protected set; }
        public GunBase(string name) : base(name)
        {
            
        }

        public override void StartUp()
        {
            TimeRemainig = 0;
            base.StartUp();
        }

        public override void Update(IGameObjectViewHost world, IGameTimer gameTimer)
        {
            if (TimeRemainig > 0)
            {
                TimeRemainig -= ReloadSpeed * (float)gameTimer.deltaTime.TotalSeconds;
            }

            if (TimeRemainig < 0)
                TimeRemainig = 0;

            base.Update(world, gameTimer);
        }

        protected virtual void Reload()
        {
            TimeRemainig = ReloadTime;
        }

        public virtual void Shoot(IMapableObjectViewHost world, Vector2 dir)
        {
            if (!GunLoaded)
                return;

            var position = GetWorldCenter();
            var shell = world.Instantinate<TShell>();
            shell.Scale(Transform.Scale * ShellScaleMultipl);
            SizeF shellSize = shell.GetActualSize();
            Vector2 centerPos = position - new Vector2(shellSize.Width / 2, shellSize.Height / 2);
            shell.Translate(centerPos);
            shell.Fire(dir);
            shell.Rotate(Transform.Rotation);
            Reload();
        }
    }
}
