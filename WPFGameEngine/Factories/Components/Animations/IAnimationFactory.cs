using WPFGameEngine.Factories.Base;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.WPF.GE.Component.Animations;

namespace WPFGameEngine.Factories.Components.Animations
{
    public interface IAnimationFactory : IFactory<IAnimation>
    {
        IResourceLoader ResourceLoader { get; }
    }
}
