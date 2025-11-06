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

            m_transform.Position = new Vector2(100,200);
            Scale(new SizeF(0.5f, 0.5f));
            m_transform.Rotation = -90;

            m_controller = (WPFInputController)GetComponent<ControllerComponent>(false);
        }

        public override void Update(List<IGameObject> world, IGameTimer gameTimer)
        {
            if (m_controller.Sender != null)
            {
                var key = m_controller.KeyEvents.Key;
                var state = m_controller.KeyEvents.KeyStates;
                var delta = gameTimer.deltaTime;
                var basis = m_transform.GetBasis(CenterPosition);
                var currPosition = m_transform.Position;
                switch (key)
                {
                    case Key.A:
                        m_transform.Position = new Vector2()
                        {
                            X = currPosition.X,
                            Y = currPosition.Y - basis.GetNormalY().Y * (float)delta.TotalSeconds,
                        };
                        break;
                    case Key.D:
                        m_transform.Position = new Vector2()
                        {
                            X = currPosition.X,
                            Y = currPosition.Y + basis.GetNormalY().Y * (float)delta.TotalSeconds,
                        };
                        break;
                    case Key.W:
                        break;
                    case Key.S:
                        break;
                }
            }

            base.Update(world, gameTimer);
        }

        #endregion
    }
}
