using SpaceAvenger.Game.Core.Spaceships.Base;
using System.Numerics;
using System.Drawing;
using Microsoft.VisualBasic;
using SpaceAvenger.Services.WPFInputControllers;
using System.Collections.Generic;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.Timers.Base;
using System.Windows.Input;
using System.Windows.Controls;
using SpaceAvenger.Extensions.Math;
using WPFGameEngine.WPF.GE.Component.Controllers;
using System.Diagnostics;

namespace SpaceAvenger.Game.Core.Spaceships.F10.Destroyer
{
    public class F10Destroyer : SpaceShipBase
    {
        WPFInputController m_controller;

        public F10Destroyer() : base(nameof(F10Destroyer))
        {

        }

        #region Methods
        public override void StartUp()
        {
            base.StartUp();

            m_transform.Position = new Vector2(100, 200);
            Scale(new SizeF(0.7f, 0.7f));
            m_transform.Rotation = -90;

            m_controller = (WPFInputController)GetComponent<ControllerComponent>(false);
        }

        public override void Update(List<IGameObject> world, IGameTimer gameTimer)
        {
            if (m_controller != null)
            {
                var delta = gameTimer.deltaTime;
                var basis = m_transform.GetBasis(CenterPosition);
                var currPosition = m_transform.Position;
                var xL = basis.GetNormalX();
                var yL = basis.GetNormalY();
                var curr = m_transform.Position;
                var maxWidth = App.Current.MainWindow.Width;
                var maxHeight = App.Current.MainWindow.Height;

                if (m_controller.IsKeyDown(Key.A) && curr.X >= 0)
                {
                    Translate(curr - (yL * (float)delta.TotalSeconds * m_horSpeed));
                }
                if (m_controller.IsKeyDown(Key.D) && curr.X < maxWidth - ActualSize.Width)
                {
                    Translate(curr + (yL * (float)delta.TotalSeconds * m_horSpeed));
                }
                if (m_controller.IsKeyDown(Key.W) && curr.Y >= 0)
                {
                    Translate(m_transform.Position = curr + (xL * (float)delta.TotalSeconds * m_vertSpeed));
                }
                if (m_controller.IsKeyDown(Key.S) && curr.Y < maxHeight - ActualSize.Height)
                {
                    Translate(curr - (xL * (float)delta.TotalSeconds * m_vertSpeed));
                }
            }

            base.Update(world, gameTimer);
        }

        #endregion
    }
}
