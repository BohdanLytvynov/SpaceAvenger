using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace SpaceAvenger.Views.Pages
{
    /// <summary>
    /// Interaction logic for Game_Page.xaml
    /// </summary>
    public partial class Game_Page : Page
    {
        public event EventHandler<InputEventArgs> OnInputFired;

        public Game_Page()
        {
            InitializeComponent();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            OnInputFired?.Invoke(sender, e);
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnInputFired?.Invoke(sender, e);
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OnInputFired?.Invoke(sender, e);
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            OnInputFired?.Invoke(sender, e);
        }

        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            OnInputFired?.Invoke(sender, e);
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            OnInputFired?.Invoke(sender, e);
        }
    }
}
