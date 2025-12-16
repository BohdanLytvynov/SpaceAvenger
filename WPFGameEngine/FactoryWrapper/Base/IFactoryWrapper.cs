using WPFGameEngine.Factories.Base;
using WPFGameEngine.Services.Interfaces;

namespace WPFGameEngine.FactoryWrapper.Base
{
    public interface IFactoryWrapper
    {
        Dictionary<string, IFactory> ProductFactoryMap { get; }
        IResourceLoader ResourceLoader { get; }
        TType CreateObject<TType>() where TType : IGameEngineEntity;
        object CreateObject(string typeName);
    }
}
