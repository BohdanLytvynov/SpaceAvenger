using SpaceAvenger.Game.Core.AI;
using SpaceAvenger.Game.Core.Enums;
using SpaceAvenger.Services.WPFInputControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows.Input;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Controllers;
using WPFGameEngine.WPF.GE.Math.Basis;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class Moveable_Explosive_SpaceShipBase<TJetType, TExplosion>
        : ExplosiveSpaceShipBase<TExplosion>
        where TExplosion : ExplosionBase
        where TJetType : JetBase
    {
        private float m_PlayerMinX;
        private float m_PlayerMinY;

        private float m_PlayerMaxX;
        private float m_PlayerMaxY;

        private IEnumerable<TJetType?>? m_mainEngines;
        private IEnumerable<TJetType?>? m_rightAccelerators;
        private IEnumerable<TJetType?>? m_leftAccelerators;

        public abstract List<string> MainEnginesNames 
        { get; }

        public abstract List<string> RightAcceleratorsNames 
        { get; }

        public abstract List<string> LeftAcceleratorsNames 
        { get; }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            base.StartUp(viewHost, gameTimer);

            var allEngines = GetAllChildrenOfType<TJetType>();

            m_leftAccelerators = allEngines.Where(e => LeftAcceleratorsNames.Contains(e.UniqueName!));

            m_rightAccelerators = allEngines.Where(e => RightAcceleratorsNames.Contains(e.UniqueName!));

            m_mainEngines = allEngines.Where(e => MainEnginesNames.Contains(e.UniqueName!));

            //Set Window Bounds
            var w = App.Current.MainWindow;
            var wScale = GetWorldScale();

            m_controller = (WPFInputController)GetComponent<ControllerComponent>(false);

            if (m_controller != null)
            {
                m_PlayerMinX = 0f;
                m_PlayerMaxX = (float)w.ActualWidth - wScale.Width;

                m_PlayerMinY = 1f / 4f * (float)w.Height;
                m_PlayerMaxY = (float)w.ActualHeight - (wScale.Height + 50f);
            }
        }

        protected Moveable_Explosive_SpaceShipBase(Faction faction) : base(faction)
        {

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
                    translateVector += MoveLeft(basis, timeDelta);
                }

                if (m_controller.IsKeyDown(Key.D))
                {
                    translateVector += MoveRight(basis, timeDelta);
                }

                if (m_controller.IsKeyDown(Key.W))
                {
                    translateVector += MoveForward(basis, timeDelta);
                    isMoving = true;
                }

                if (!isMoving)
                {
                    translateVector += MoveBackward(basis, timeDelta);
                    StopAllEngines();
                }

                Vector2 newPos = curr + translateVector;

                float clampedX = Math.Clamp(newPos.X, m_PlayerMinX, m_PlayerMaxX);
                float clampedY = Math.Clamp(newPos.Y, m_PlayerMinY, m_PlayerMaxY);

                Vector2 finalPos = new Vector2(clampedX, clampedY);

                Translate(finalPos);
            }

            base.Update();
        }

        protected virtual void StopAllEngines()
        {
            StopAll(m_mainEngines!);
            StopAll(m_leftAccelerators!);
            StopAll(m_rightAccelerators!);
        }

        private void StopAll(IEnumerable<JetBase>? jets)
        {
            if (jets == null) return;

            foreach (var jet in jets)
            {
                jet.Stop();
            }
        }

        protected virtual Vector2 MoveForward(Basis2D basis, float timeDelta)
        {
            foreach (var item in m_mainEngines!)
            {
                item.Start();
            }

            return basis.X * timeDelta * VertSpeed;
        }

        protected virtual Vector2 MoveBackward(Basis2D basis, float timeDelta)
        {
            foreach (var item in m_mainEngines!)
            {
                item.Stop();
            }

            return -(basis.X * timeDelta * VertSpeed);
        }

        protected virtual Vector2 MoveLeft(Basis2D basis, float timeDelta)
        {
            foreach (var item in m_rightAccelerators!)
            {
                item.Start();
            }

            return -(basis.Y * timeDelta * HorSpeed);
        }

        protected virtual Vector2 MoveRight(Basis2D basis, float timeDelta)
        {
            foreach (var item in m_leftAccelerators!)
            {
                item.Start();
            }

            return basis.Y * timeDelta * HorSpeed;
        }

        protected virtual float GetTime(float distance)
        {
            return distance / HorSpeed;
        }

        public override void ConfigureAI(SpaceShipControlModule aIModule)
        {
            aIModule.MoveLeft = MoveLeft;
            aIModule.MoveRight = MoveRight;
            aIModule.MoveForward = MoveForward;
            aIModule.MoveBackward = MoveBackward;
            aIModule.GetTime = GetTime;

            AIModule = aIModule ?? throw new ArgumentNullException(nameof(aIModule));
        }
    }
}