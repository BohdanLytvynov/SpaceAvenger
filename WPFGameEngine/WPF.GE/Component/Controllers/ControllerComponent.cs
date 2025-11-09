using System.Numerics;
using System.Windows.Input;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Dto.Components;

namespace WPFGameEngine.WPF.GE.Component.Controllers
{
    public abstract class ControllerComponent : ComponentBase, IControllerComponent
    {
        #region Properties
        public override List<string> IncompatibleComponents => new List<string>();

        public Vector2 MousePosition { get; protected set; }

        public MouseButton MouseButton { get; protected set; }
        #endregion

        #region Ctor
        public ControllerComponent() : base(nameof(ControllerComponent))
        {}
        #endregion

        #region Methods
        public override ControllerDto ToDto()
        {
            throw new NotImplementedException();
        }

        public abstract void Dispose();

        public abstract bool IsKeyDown(Key key);

        public abstract bool IsMouseButtonDown(MouseButton mouseButton);
        #endregion
    }
}
