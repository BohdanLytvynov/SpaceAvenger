using SpaceAvenger.Services.WPFInputControllers;
using System.Collections.Generic;
using System.Windows.Input;
using WPFGameEngine.Extensions;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.ObjectBuilders.Base;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Spaceships.Base
{
    public abstract class SpaceShipBase : MapableObject
    {
        protected WPFInputController m_controller;
        protected TransformComponent m_transform;
        protected Sprite m_sprite;
        protected Animator m_animator;

        #region Fields
        protected float m_horSpeed;
        protected float m_vertSpeed;
        #endregion

        protected SpaceShipBase(string name) : base(name)
        {
            m_horSpeed = 40;
            m_vertSpeed = 40;
        }

        public override void StartUp()
        {
            m_transform = GetComponent<TransformComponent>();
            m_sprite = GetComponent<Sprite>();
        }

        public override void Update(IGameViewHost world, IGameTimer gameTimer)
        {
            if (m_controller != null)
            {
                var delta = gameTimer.deltaTime;
                var basis = m_transform.GetLocalTransformMatrix().GetBasis();
                var currPosition = m_transform.Position;
                var curr = m_transform.Position;
                var maxWidth = App.Current.MainWindow.Width;
                var maxHeight = App.Current.MainWindow.Height;
                var ActualSize = m_transform.ActualSize;
                if (m_controller.IsKeyDown(Key.A) && curr.X >= 0)
                {
                    Translate(curr - (basis.Y * (float)delta.TotalSeconds * m_horSpeed));
                }
                if (m_controller.IsKeyDown(Key.D) && curr.X < maxWidth - ActualSize.Width)
                {
                    Translate(curr + (basis.Y * (float)delta.TotalSeconds * m_horSpeed));
                }
                if (m_controller.IsKeyDown(Key.W) && curr.Y >= 0)
                {
                    Translate(m_transform.Position = curr + (basis.X * (float)delta.TotalSeconds * m_vertSpeed));
                }
                if (m_controller.IsKeyDown(Key.S) && curr.Y < maxHeight - ActualSize.Height)
                {
                    Translate(curr - (basis.X * (float)delta.TotalSeconds * m_vertSpeed));
                }
            }

            base.Update(world, gameTimer);
        }
    }
}
