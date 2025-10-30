namespace WPFGameEngine.Factories.Base
{
    public abstract class FactoryBase : IFactory
    {
        public string? ProductName { get; init; }

        public abstract IGameEngineEntity Create();
    }
}
