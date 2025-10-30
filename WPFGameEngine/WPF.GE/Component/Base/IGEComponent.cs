using WPFGameEngine.Factories.Base;

namespace WPFGameEngine.WPF.GE.Component.Base
{
    public interface IGEComponent : IGameEngineEntity
    {
        string ComponentName { get; init; }
    }
}
