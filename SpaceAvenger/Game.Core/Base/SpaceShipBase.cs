using SpaceAvenger.Services.WPFInputControllers;
using System.Windows.Input;
using WPFGameEngine.Extensions;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class SpaceShipBase : MapableObject
    {
        protected WPFInputController m_controller;
        
        #region Fields
        public float HorSpeed { get; protected set; }
        public float VertSpeed { get; protected set; }
        #endregion

        protected SpaceShipBase(string name) : base(name)
        {
        }
   
        public override void Update(IGameObjectViewHost world, IGameTimer gameTimer)
        {
            if (m_controller != null)
            {
                var delta = gameTimer.deltaTime;
                var basis = Transform.GetLocalTransformMatrix().GetBasis();
                var currPosition = Transform.Position;
                var curr = Transform.Position;
                var maxWidth = System.Windows.Application.Current.MainWindow.Width;
                var maxHeight = System.Windows.Application.Current.MainWindow.Height;
                var ActualSize = Transform.ActualSize;
                if (m_controller.IsKeyDown(Key.A) && curr.X >= 0)
                {
                    Translate(curr - basis.Y * (float)delta.TotalSeconds * HorSpeed);
                }
                if (m_controller.IsKeyDown(Key.D) && curr.X < maxWidth - ActualSize.Width)
                {
                    Translate(curr + basis.Y * (float)delta.TotalSeconds * HorSpeed);
                }
                if (m_controller.IsKeyDown(Key.W) && curr.Y >= 0)
                {
                    Translate(Transform.Position = curr + basis.X * (float)delta.TotalSeconds * VertSpeed);
                }
                if (m_controller.IsKeyDown(Key.S) && curr.Y < maxHeight - ActualSize.Height)
                {
                    Translate(curr - basis.X * (float)delta.TotalSeconds * VertSpeed);
                }
            }

            base.Update(world, gameTimer);
        }
    }
}
