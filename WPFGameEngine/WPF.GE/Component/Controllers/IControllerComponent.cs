using System.Drawing;
using WPFGameEngine.WPF.GE.Component.Base;

namespace WPFGameEngine.WPF.GE.Component.Controllers
{
    public interface IControllerComponent : IGEComponent
    {
        public object Sender { get; }
        
        void OnInputFired(object sender, EventArgs e);
    }
}
