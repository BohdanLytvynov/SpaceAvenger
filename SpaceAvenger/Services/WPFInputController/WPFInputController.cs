using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
using WPFGameEngine.WPF.GE.Component.Controllers;

namespace SpaceAvenger.Services.WPFInputControllers
{
    internal class WPFInputController : ControllerComponent
    {
        public PointF MousePosition { get; set; }
        public MouseEventArgs MouseEvents { get; set; }
        public MouseButtonEventArgs MouseButtonEvents { get; set; }
        public KeyEventArgs KeyEvents { get; set; }
        public MouseWheelEventArgs MouseWheelEvents { get; set; }

        public override void OnInputFired(object sender, EventArgs e)
        {
            if (e is MouseEventArgs mea)//Get mouse position
            {
                var p = mea.GetPosition((Canvas)sender);
                MousePosition = new PointF((float)p.X, (float)p.Y);
                MouseEvents = mea;
            }
            else if (e is MouseButtonEventArgs mbea)
            {
                MouseButtonEvents = mbea;
            }
            else if (e is KeyEventArgs kea)
            {
                KeyEvents = kea;
            }
            else if (e is MouseWheelEventArgs mwea)
            { 
                MouseWheelEvents = mwea;
            }
        }
    }
}
