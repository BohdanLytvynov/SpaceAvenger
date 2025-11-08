using SpaceAvenger.Game.Core.Spaceships.Base;
using System.Numerics;
using System.Drawing;
using SpaceAvenger.Services.WPFInputControllers;
using System.Collections.Generic;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Controllers;
using SpaceAvenger.Game.Core.Spaceships.F10.Weapons;
using System.Windows.Media;
using SpaceAvenger.Extensions.Math;
using WPFGameEngine.Extensions;
using System;
using System.Windows.Input;

namespace SpaceAvenger.Game.Core.Spaceships.F10.Destroyer
{
    public class F10Destroyer : SpaceShipBase
    {
        private F10RailGun m_gun1;
        private Pen m_targetMarker;
        public F10Destroyer() : base(nameof(F10Destroyer))
        {

        }

        #region Methods
        public override void StartUp()
        {
            base.StartUp();
            m_targetMarker = new Pen();
            m_transform.Position = new Vector2(100, 200);
            Scale(new SizeF(0.7f, 0.7f));
            m_transform.Rotation = -90;
            m_targetMarker = new Pen() { Brush = Brushes.Orange };
            m_targetMarker.Freeze();
            m_controller = (WPFInputController)GetComponent<ControllerComponent>(false);

            m_gun1 = (F10RailGun)this.FindChild(o => o.UniqueName.Equals("RailGun1"));
        }

        public override void Update(List<IGameObject> world, IGameTimer gameTimer)
        {
            //var m = GetGlobalMatrix(this, m_gun1);
            //var m_gun1Pos = m.GetTranslate();

            //var dirVector = m_controller.MousePosition - m_gun1Pos;

            //double angle = Math.Atan2(dirVector.Y, dirVector.X);

            //m_gun1.Rotate(angle * 180 / Math.PI);

            base.Update(world, gameTimer);
        }

        public override void Render(DrawingContext dc, Matrix parent = default)
        {
            base.Render(dc, parent);

            var m = m_gun1.GetGlobalTransformMatrix();
            var b = m.GetBasis();
            var center = m_gun1.GetTransformComponent().ActualCenterPosition;
            var lx = b.X.Multiply(center.X);
            var ly = b.Y.Multiply(center.Y);
            var l = lx + ly;


            dc.DrawEllipse(Brushes.Blue, new Pen(), (m.GetTranslateAsVector()).ToPoint(), 2,2);


        }

        #endregion
    }
}
