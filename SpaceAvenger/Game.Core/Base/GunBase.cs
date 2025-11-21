using SpaceAvenger.Game.Core.Factions.F10.Projectiles;
using System;
using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace SpaceAvenger.Game.Core.Base
{
    public class GunBase<TShell> : MapableObject
        where TShell : ProjectileBase
    {
        protected Brush GunReady;
        protected Brush GunLoadedHalf;
        protected Brush GunUnloaded;

        public float TimeRemainig { get; private set; }
        //Seconds
        public float ReloadSpeed { get; protected set; }
        public float ReloadTime { get; protected set; }
        public bool GunLoaded { get => TimeRemainig <= 0; }
        public float ShellScaleMultipl { get; protected set; }
        public GunBase(string name) : base(name)
        {
            
        }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            GunReady = Brushes.Green;
            GunLoadedHalf = Brushes.Orange;
            GunUnloaded = Brushes.Red;

            TimeRemainig = 0;
            base.StartUp(viewHost, gameTimer);
        }

        public override void Update()
        {
            if (TimeRemainig > 0)
            {
                TimeRemainig -= ReloadSpeed * (float)GameTimer.deltaTime.TotalSeconds;
            }

            if (TimeRemainig < 0)
                TimeRemainig = 0;

            base.Update();
        }

        protected virtual void Reload()
        {
            TimeRemainig = ReloadTime;
        }

        public virtual void Shoot(Vector2 dir)
        {
            if (!GunLoaded)
                return;

            var position = GetWorldCenter();
            var shell = (GameView as IMapableObjectViewHost).Instantiate<TShell>();
            shell.Scale(Transform.Scale * ShellScaleMultipl);
            Size shellSize = shell.GetActualSize();
            Vector2 centerPos = position - new Vector2(shellSize.Width / 2, shellSize.Height / 2);
            shell.Translate(centerPos);
            shell.Fire(dir);
            var angle = Math.Atan2(dir.Y, dir.X) * 180/Math.PI;
            shell.Rotate(Transform.Rotation);//Not so good logic
            Reload();
        }

        protected virtual Brush GetLoadIndicatorColor(float value)
        {
            if (value >= 0 && value < 0.25f)
                return GunReady;
            if (value >= 0.25f && value < 0.75f)
                return GunLoadedHalf;
            else
                return GunUnloaded;
        }
    }
}
