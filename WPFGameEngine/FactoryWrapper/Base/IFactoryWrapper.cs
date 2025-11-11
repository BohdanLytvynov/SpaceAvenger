using WPFGameEngine.Factories.Base;
using WPFGameEngine.Factories.Components.Animations;
using WPFGameEngine.Factories.Components.Animators;
using WPFGameEngine.Factories.Components.RelativeTransforms;
using WPFGameEngine.Factories.Components.Sprites;
using WPFGameEngine.Factories.Components.Transform;
using WPFGameEngine.Factories.Ease;
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
