namespace WPFGameEngine.Factories.Base
{
    public interface IFactory
    {
        string? ProductName { get; }

        IGameEngineEntity Create();
    }
}
