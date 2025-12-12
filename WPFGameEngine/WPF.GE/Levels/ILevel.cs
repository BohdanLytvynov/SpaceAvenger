using WPFGameEngine.WPF.GE.Component.Controllers;
using WPFGameEngine.WPF.GE.GameObjects.Updatable;

namespace WPFGameEngine.WPF.GE.Levels
{
    public interface ILevel : IUpdatable
    {
        IControllerComponent? ControllerComponent { get; set; }
    }
}
