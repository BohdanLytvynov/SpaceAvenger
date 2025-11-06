using System.Drawing;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Dto.Components;

namespace WPFGameEngine.WPF.GE.Component.Controllers
{
    public abstract class ControllerComponent : ComponentBase, IControllerComponent
    {
        #region Properties

        public override List<string> IncompatibleComponents => new List<string>();

        public object Sender { get; private set; }
        #endregion

        #region Ctor
        public ControllerComponent() : base(nameof(ControllerComponent))
        {
            
        }
        #endregion

        #region Methods
        public override ControllerDto ToDto()
        {
            return new ControllerDto();
        }

        public abstract void OnInputFired(object sender, EventArgs e);

        #endregion
    }
}
