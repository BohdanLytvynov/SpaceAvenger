using System.Windows;
using System.Windows.Input;
using WPFGameEngine.WPF.GE.Component.Base;

namespace WPFGameEngine.WPF.GE.Component.Controllers
{
    public interface IControllerComponent : IGEComponent, IDisposable
    {
        Point MousePosition { get; }

        MouseButton MouseButton { get; }

        bool IsKeyDown(Key key);
    }
}
