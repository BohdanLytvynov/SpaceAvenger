using WPFGameEngine.Factories.Base;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.WPF.GE.Component.Animations;

namespace WPFGameEngine.Factories.Components.Animations
{
    internal class AnimationFactory : FactoryBase<IAnimation>, IAnimationFactory
    {
        public AnimationFactory(IResourceLoader resourceLoader) : base()
        {
            ResourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
        }

        public IResourceLoader ResourceLoader { get; protected set; }

        public override IAnimation Create()
        {
            return new Animation(ResourceLoader);
        }
    }
}
