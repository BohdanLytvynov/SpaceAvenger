using SpaceAvenger.Game.Core.Enums;
using SpaceAvenger.Game.Core.UI.Slider;
using SpaceAvenger.Services.WPFInputControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows.Input;
using System.Windows.Media;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class SpaceShipBase : СacheableObject
    {
        #region Fields
        private bool m_moveForward;

        private float MinX;
        private float MinY;

        private float MaxX;
        private float MaxY;
        #endregion

        protected WPFInputController m_controller;

        #region Properties
        public float HP { get; set; }
        public float Shield { get; set; }
        public float HorSpeed { get; protected set; }
        public float VertSpeed { get; protected set; }
        public Faction Faction { get; private set; }
        public bool IsAlive { get; private set; }

        protected Bar HPBar;
        protected Bar ShieldBar;

        protected Brush BarLow;
        protected Brush BarHigh;
        protected Brush BarMedium;

        protected IEnumerable<JetBase> m_Engines;

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

            var w = App.Current.MainWindow;

            MinX = 0f;
            MaxX = (float)w.Width - this.Transform.ActualSize.Width;

            MinY = 1f/4f * (float)w.Height;
            MaxY = (float)w.Height - (this.Transform.ActualSize.Height + 50f);
        }

        public override void Update()
        {
            if (m_controller != null)
            {
                var delta = GameTimer.deltaTime;
                var basis = Transform.GetLocalTransformMatrix().GetBasis();
                var curr = Transform.Position;

                Vector2 translateVector = Vector2.Zero;
                float timeDelta = (float)delta.TotalSeconds;

                // --- 1. Обработка Ввода (W, A, D) ---
                bool isMoving = false;

                // Движение ВЛЕВО (A)
                if (m_controller.IsKeyDown(Key.A))
                {
                    // Предполагая, что Y - это горизонтальная ось (влево/вправо)
                    translateVector -= basis.Y * timeDelta * HorSpeed;
                    MoveLeft();
                }

                // Движение ВПРАВО (D)
                if (m_controller.IsKeyDown(Key.D))
                {
                    translateVector += basis.Y * timeDelta * HorSpeed;
                    MoveRight();
                }

                // Движение ВПЕРЕД (W)
                if (m_controller.IsKeyDown(Key.W))
                {
                    // Предполагая, что X - это вертикальная ось (вперед/назад)
                    translateVector += basis.X * timeDelta * VertSpeed;
                    isMoving = true;
                    MoveForward();
                }

                // --- 2. Автоматическое Опускание при Отпускании Клавиши ---

                // Если "Вперед" не нажата
                if (!isMoving)
                {
                    // Корабль опускается (двигается вниз по оси X)
                    // Использование VertSpeed для опускания, но возможно, нужна отдельная скорость AutoDownSpeed
                    translateVector -= basis.X * timeDelta * VertSpeed;
                    StopAllEngines();
                }

                // --- 3. Вычисление Новой Позиции ---

                Vector2 newPos = curr + translateVector;

                // --- 4. Принудительное Ограничение Позиции ---

                // Предполагая, что у вас есть две константы: MinX и MaxX для вертикальных границ
                // и MinY и MaxY для горизонтальных границ окна

                float clampedX = Math.Clamp(newPos.X, MinX, MaxX);
                float clampedY = Math.Clamp(newPos.Y, MinY, MaxY);

                Vector2 finalPos = new Vector2(clampedX, clampedY);

                // --- 5. Применение Позиции ---

                Translate(finalPos);
            }
            else
            {
                //Base Enemy AI
            }

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

        protected abstract void MoveForward();
        protected abstract void MoveBackward();
        protected abstract void MoveLeft();
        protected abstract void MoveRight();
        protected virtual void StopAllEngines()
        {
            foreach (var item in m_Engines)
            {
                item.Stop();
            }
        }
    }
}
