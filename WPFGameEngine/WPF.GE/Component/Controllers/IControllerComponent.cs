using System.Numerics;
using System.Windows.Input;
using WPFGameEngine.WPF.GE.Component.Base;

namespace WPFGameEngine.WPF.GE.Component.Controllers
{
    public interface IControllerComponent : IGEComponent, IDisposable
    {
        Vector2 MousePosition { get; }

        MouseButton MouseButton { get; }

        bool IsKeyDown(Key key);
        void ReleaseKey(Key key);

        bool IsMouseButtonDown(MouseButton mouseButton);
    }
}
