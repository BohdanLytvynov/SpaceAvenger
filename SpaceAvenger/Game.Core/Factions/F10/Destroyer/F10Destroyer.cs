using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Factions.F10.Weapons;
using SpaceAvenger.Services.WPFInputControllers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.Extensions;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.ObjectBuilders.Base;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Controllers;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Factions.F10.Destroyer
{
    public class F10Destroyer : SpaceShipBase
    {
        private F10RailGun m_gun1;
        private Pen m_targetMarkerPen;
        public F10Destroyer() : base(nameof(F10Destroyer))
        {
            HorSpeed = 40;
            VertSpeed = 40;
        }

        #region Methods
        public override void StartUp()
        {
            base.StartUp();
            m_targetMarkerPen = new Pen();
            Transform.Position = new Vector2(100, 200);
            Scale(new SizeF(0.5f, 0.5f));
            Transform.Rotation = -90;
            m_targetMarkerPen = new Pen() { Brush = Brushes.Orange };
            m_targetMarkerPen.Freeze();
            m_controller = (WPFInputController)GetComponent<ControllerComponent>(false);

            m_gun1 = (F10RailGun)FindChild(o => o.UniqueName.Equals("RailGun1"));
        }

        public override void Update(IGameObjectViewHost world, IGameTimer gameTimer)
        {
            if (m_controller != null)
            {
                m_gun1.LookAt(m_controller.MousePosition, 2, gameTimer.deltaTime.TotalSeconds);

                if (m_controller.IsKeyDown(System.Windows.Input.Key.R))
                {
                    Rotate(Transform.Rotation + 5);
                }

                if (m_controller.IsMouseButtonDown(System.Windows.Input.MouseButton.Left))
                {
                    m_gun1.Shoot(world as IMapableObjectViewHost, GetDirection(m_controller.MousePosition));
                }
            }

            base.Update(world, gameTimer);
        }

        public override void Render(DrawingContext dc, Matrix parent = default)
        {
            base.Render(dc, parent);
            
            dc.DrawLine(m_targetMarkerPen, 
                m_gun1.GetWorldCenter().ToPoint(), 
                m_controller.MousePosition.ToPoint());

        }

        #endregion
    }
}
