using SpaceAvenger.Game.Core.Enums;
using SpaceAvenger.Services.WPFInputControllers;
using System.Windows.Input;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class SpaceShipBase : MapableObject
    {
        protected WPFInputController m_controller;

        #region Properties
        public float HP { get; set; }
        public float Shield { get; set; }
        public float HorSpeed { get; protected set; }
        public float VertSpeed { get; protected set; }
        public Faction Faction { get; private set; }
        #endregion

        protected SpaceShipBase(Faction factionName, string name) : base(name)
        {
            Faction = factionName;
        }
   
        public override void Update()
        {
            if (m_controller != null)
            {
                var delta = GameTimer.deltaTime;
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

            base.Update();
        }
    }
}
