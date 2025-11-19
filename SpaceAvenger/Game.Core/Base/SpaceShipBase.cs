using SpaceAvenger.Game.Core.Enums;
using SpaceAvenger.Game.Core.UI.Slider;
using SpaceAvenger.Services.WPFInputControllers;
using System.Windows.Input;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class SpaceShipBase : СacheableObject
    {
        protected WPFInputController m_controller;

        #region Properties
        public float HP { get; set; }
        public float Shield { get; set; }
        public float HorSpeed { get; protected set; }
        public float VertSpeed { get; protected set; }
        public Faction Faction { get; private set; }

        protected Slider1 HPSlider;
        protected Slider1 ShieldSlider;

        #endregion

        protected SpaceShipBase(Faction factionName, string name) : base(name)
        {
            Faction = factionName;
        }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            Enable(true);

            HPSlider = FindChild(x => x.UniqueName.Equals("HP")) as Slider1;
            ShieldSlider = FindChild(x => x.UniqueName.Equals("Shield")) as Slider1;

            base.StartUp(viewHost, gameTimer);
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

            HPSlider?.Update(HP);
            ShieldSlider?.Update(Shield);

            if(HP <= 0)
                Destroy();

            base.Update();
        }

        protected override void OnAddToPool()
        {
            base.OnAddToPool();
        }

        protected virtual void Destroy()
        {
            Disable(true);
        }
    }
}
