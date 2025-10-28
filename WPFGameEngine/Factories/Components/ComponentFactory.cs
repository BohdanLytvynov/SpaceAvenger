using WPFGameEngine.Factories.Base;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Component.Transforms;

namespace WPFGameEngine.Factories.Components
{
    public class ComponentFactory : IComponentFactory
    {
        public IResourceLoader ResourceLoader { get; protected set; }

        public ComponentFactory(IResourceLoader resourceLoader)
        {
            ResourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
        }

        public IGEComponent Create(string name)
        {
            IGEComponent component = null;
            switch (name)
            {
                case nameof(TransformComponent):
                    component = new TransformComponent();
                    break;
                case nameof(Sprite):
                    component = new Sprite(ResourceLoader);
                    break;
                case nameof(Animator):
                    component = new Animator();
                    break;
                case nameof(Animation):
                    component = new Animation(ResourceLoader) { Freeze = true };
                    break;
                default:
                    throw new Exception($"Unsupported component type! Type: {name}");
            }

            return component;
        }

        public IGEComponent Create<IGEComponent>()
        {
            string nane = typeof(IGEComponent).Name;

            return Create(nane);
        }
    }
}
