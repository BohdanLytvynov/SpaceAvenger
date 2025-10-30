using WPFGameEngine.Factories.Base;
using WPFGameEngine.Services.Interfaces;

namespace WPFGameEngine.Factories.Components.Animations
{
    public interface IAnimationFactory : IFactory
    {
        IResourceLoader ResourceLoader { get; }
    }
}
