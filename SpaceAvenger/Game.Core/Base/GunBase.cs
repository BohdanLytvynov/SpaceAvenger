using System;
using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace SpaceAvenger.Game.Core.Base
{
    public class GunBase<TShell, TGunBlast> : MapableObject
        where TShell : ProjectileBase
        where TGunBlast : ExplosionBase
    {
        private ExplosionBase m_Blast;
        private Vector2 m_gunCenterPosition;
        private bool m_fired;
        protected IMapableObjectViewHost MapableViewHost;
        protected Vector2 m_ShootDirection;
        private IAnimation m_blastAnimation;
        private Vector2 m_blastPosition;

        protected Brush GunReady;
        protected Brush GunLoadedHalf;
        protected Brush GunUnloaded;

        public float TimeRemainig { get; private set; }
        //Seconds
        public float ReloadSpeed { get; protected set; }
        public float ReloadTime { get; protected set; }
        public bool GunLoaded { get => TimeRemainig <= 0; }
        public float ShellScaleMultipl { get; protected set; }
        public float GunBlastScaleMultipl { get; protected set; }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            GunReady = Brushes.Green;
            GunLoadedHalf = Brushes.Orange;
            GunUnloaded = Brushes.Red;
            m_fired = false;
            TimeRemainig = 0;
            MapableViewHost = (viewHost as IMapableObjectViewHost);
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

            if (m_fired && m_blastAnimation != null && m_blastAnimation.CurrentFrameIndex 
                == m_blastAnimation.AnimationFrames.Count/4)
            {
                var shell = MapableViewHost.Instantiate<TShell>();
                shell.Scale(Transform.Scale * ShellScaleMultipl);
                Size shellSize = shell.GetActualSize();
                Vector2 centerPos = m_blastPosition - new Vector2(shellSize.Width / 2, shellSize.Height / 2);
                shell.Translate(centerPos);
                var angle = Math.Atan2(m_ShootDirection.Y, m_ShootDirection.X) * 180 / Math.PI;
                shell.Rotate(angle + 90);//Init sprite rotation to -90 deg, so we need to fix it by rotating to 90 deg
                shell.Fire(m_ShootDirection);
                m_fired = false;
                Reload();
            }

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
            m_ShootDirection = dir;
            var worldMatrix = GetWorldTransformMatrix();
            m_gunCenterPosition = GetWorldCenter(worldMatrix);

            m_Blast = (GameView as IMapableObjectViewHost).Instantiate<TGunBlast>();
            m_blastAnimation = m_Blast.GetComponent<Animation>();
            m_blastPosition = m_gunCenterPosition +
                worldMatrix.GetBasis().X * ((1 - Transform.CenterPosition.X) * Transform.ActualSize.Width);
            var angle = Math.Atan2(dir.Y, dir.X) * 180 / Math.PI;
            m_Blast.Scale(Transform.Scale * GunBlastScaleMultipl);
            m_Blast.Rotate(angle);
            m_Blast.Explode(m_blastPosition);
            m_fired = true;
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
