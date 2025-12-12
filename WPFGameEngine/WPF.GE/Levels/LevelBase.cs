using WPFGameEngine.WPF.GE.Component.Controllers;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.WPF.GE.Levels
{
    public abstract class LevelBase : UpdatableBase, ILevel
    {
        public IControllerComponent? ControllerComponent { get; set; }
    }
}
