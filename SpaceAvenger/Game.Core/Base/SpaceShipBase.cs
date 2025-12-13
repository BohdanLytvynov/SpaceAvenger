using SpaceAvenger.Game.Core.Enums;
using SpaceAvenger.Game.Core.UI.Slider;
using SpaceAvenger.Services.WPFInputControllers;
using System;
using System.Numerics;
using System.Windows.Input;
using System.Windows.Media;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Controllers;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class SpaceShipBase : СacheableObject
    {
        #region Fields

        private float m_PlayerMinX;
        private float m_PlayerMinY;

        private float m_PlayerMaxX;
        private float m_PlayerMaxY;

        private float m_MinY;
        private float m_MaxY;
        #endregion

        protected WPFInputController m_controller;

        #region Properties
        public float HP { get; set; }
        public float Shield { get; set; }
        public float ShieldRegenSpeed { get; protected set; }
        public float HorSpeed { get; protected set; }
        public float VertSpeed { get; protected set; }
        public Faction Faction { get; private set; }
        public bool IsAlive { get; private set; }

        protected Bar HPBar;
        protected Bar ShieldBar;

        protected Brush BarLow;
        protected Brush BarHigh;
        protected Brush BarMedium;

        #endregion

        protected SpaceShipBase(Faction factionName)
        {
            Faction = factionName;
        }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            Enable(true);
            IsAlive = true;
            BarLow = Brushes.Red;
            BarHigh = Brushes.Green;
            BarMedium = Brushes.Orange;

            HPBar = FindChild(x => x.UniqueName.Equals("HP")) as Bar;
            HPBar.Max = HP;
            HPBar.Low = BarLow;
            HPBar.Medium = BarMedium;
            HPBar.Full = BarHigh;

            ShieldBar = FindChild(x => x.UniqueName.Equals("Shield")) as Bar;
            if (ShieldBar != null)
            {
                ShieldBar.Max = Shield;
                ShieldBar.Low = BarLow;
                ShieldBar.Medium = BarMedium;
                ShieldBar.Full = BarHigh;
            }

            base.StartUp(viewHost, gameTimer);
            //Set Window Bounds
            var w = App.Current.MainWindow;
            var wScale = GetWorldScale();

            m_controller = (WPFInputController)GetComponent<ControllerComponent>(false);

            if (m_controller != null)
            {
                m_PlayerMinX = 0f;
                m_PlayerMaxX = (float)w.ActualWidth - wScale.Width;

                m_PlayerMinY = 1f / 4f * (float)w.ActualHeight;
                m_PlayerMaxY = (float)w.ActualHeight - (wScale.Height + 50f);
            }
            else
            {
                m_MinY = 0f - wScale.Height + 20f;
                m_MaxY = (float)w.ActualHeight + 50f;
            }
            
        }

        public override void Update()
        {
            var delta = GameTimer.deltaTime;
            float timeDelta = (float)delta.TotalSeconds;

            if (m_controller != null)//Player Controls
            {
                var basis = Transform.GetLocalTransformMatrix().GetBasis();
                var curr = Transform.Position;

                Vector2 translateVector = Vector2.Zero;

                bool isMoving = false;

                if (m_controller.IsKeyDown(Key.A))
                {
                    translateVector -= basis.Y * timeDelta * HorSpeed;
                    MoveLeft();
                }

                if (m_controller.IsKeyDown(Key.D))
                {
                    translateVector += basis.Y * timeDelta * HorSpeed;
                    MoveRight();
                }

                if (m_controller.IsKeyDown(Key.W))
                {
                    translateVector += basis.X * timeDelta * VertSpeed;
                    isMoving = true;
                    MoveForward();
                }

                if (!isMoving)
                {
                    translateVector -= basis.X * timeDelta * VertSpeed;
                    StopAllEngines();
                }

                Vector2 newPos = curr + translateVector;

                float clampedX = Math.Clamp(newPos.X, m_PlayerMinX, m_PlayerMaxX);
                float clampedY = Math.Clamp(newPos.Y, m_PlayerMinY, m_PlayerMaxY);

                Vector2 finalPos = new Vector2(clampedX, clampedY);

                Translate(finalPos);
            }
            else
            {
                //Base Enemy AI

                if (Transform.Position.Y >= m_MinY && Transform.Position.Y <= m_MaxY)
                {

                }
            }

            Shield += ShieldRegenSpeed * timeDelta;

            HPBar.Update(HP);
            ShieldBar?.Update(Shield);

            if (HP <= 0 && IsAlive)
            {
                IsAlive = false;
                Destroy();
            }

            base.Update();
        }

        protected virtual void Destroy()
        {
            Disable(true);
            AddToPool(this);
        }

        public override void OnGetFromPool()
        {
            IsAlive = true;
            base.OnGetFromPool();
        }

        public virtual void DoDamage(float damage)
        {
            if (Shield == 0f || Shield - damage <= 0f)
            {
                HP -= damage;
            }
            else
            { 
                Shield -= damage;
            }
        }

        protected virtual void MoveForward() { }
        protected virtual void MoveBackward() { }
        protected virtual void MoveLeft() { }
        protected virtual void MoveRight() { }
        protected virtual void StopAllEngines() { }
    }
}