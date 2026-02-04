using SpaceAvenger.Extensions.Math;
using System;
using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Math.Matrixes;

namespace SpaceAvenger.Game.Core.Base
{
    public class GunBase<TShell, TGunBlast> : WeaponBase
        where TShell : ProjectileBase
        where TGunBlast : ExplosionBase
    {
        private ExplosionBase? m_Blast;
        private bool m_fired;
        protected Vector2 m_ShootDirection;
        private IAnimation? m_blastAnimation;

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
        public float XAxisGunBlastPositionMultipl { get; protected set; }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            GunReady = Brushes.Green;
            GunLoadedHalf = Brushes.Orange;
            GunUnloaded = Brushes.Red;
            m_fired = false;
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

            if (m_fired && m_blastAnimation != null && m_blastAnimation.CurrentFrameIndex
                == m_blastAnimation.AnimationFrames.Count / 4)
            {
                var shell = MapableViewHost.Instantiate<TShell>();
                shell.CollisionLayer = ProjectilesCollisionLayer;
                shell.Scale(Transform.Scale * ShellScaleMultipl);
                var blastPos = GetBlastPosition();
                Vector2 shellCenterPos = blastPos - shell.Transform.TextureCenterPosition;
                m_Blast.UpdatePosition(blastPos);
                shell.Translate(shellCenterPos);
                var angle = Math.Atan2(m_ShootDirection.Y, m_ShootDirection.X) * 180 / Math.PI;
                shell.Rotate(angle);
                shell.Fire(m_ShootDirection);
                m_fired = false;
                Reload();
            }

            if (m_Blast != null && m_blastAnimation.CurrentFrameIndex <= 
                m_blastAnimation.AnimationFrames.Count * 1/3)
            {
                m_Blast.UpdatePosition(GetBlastPosition());
            }

            base.Update();
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

        protected virtual void Reload()
        {
            TimeRemainig = ReloadTime;
        }

        public override void Shoot(Vector2 dir)
        {
            if (!GunLoaded || m_fired)
                return;
            m_ShootDirection = dir;
            m_Blast = MapableViewHost.Instantiate<TGunBlast>();
            m_blastAnimation = m_Blast.GetComponent<Animation>();
            var angle = Math.Atan2(dir.Y, dir.X) * 180 / Math.PI;
            m_Blast.Scale(Transform.Scale * GunBlastScaleMultipl);
            m_Blast.Rotate(angle);
            m_Blast.Explode(GetBlastPosition());
            m_fired = true;
        }

        private Vector2 GetBlastPosition()
        {
            var worldMatrix = GetWorldTransformMatrix();
            var basis = worldMatrix.GetBasis();
            var actSize = GetWorldScale(worldMatrix);
            var center = GetWorldCenter(worldMatrix);
            return center + basis.X * (((1 - Transform.CenterPosition.X) * actSize.Width) + 
                XAxisGunBlastPositionMultipl);
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
