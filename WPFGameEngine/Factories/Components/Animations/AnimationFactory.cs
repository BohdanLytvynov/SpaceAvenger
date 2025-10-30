using WPFGameEngine.Factories.Base;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.WPF.GE.Component.Animations;

namespace WPFGameEngine.Factories.Components.Animations
{
    internal class AnimationFactory : FactoryBase, IAnimationFactory
    {
        public AnimationFactory(IResourceLoader resourceLoader)
        {
            ResourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
            ProductName = nameof(Animation);
        }

        public IResourceLoader ResourceLoader { get; protected set; }

        public override IAnimation Create()
        {
            return new Animation(ResourceLoader);
        }
    }
}
