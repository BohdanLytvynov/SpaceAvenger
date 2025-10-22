using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Component.Transforms;

namespace WPFGameEngine.Factories.Components
{
    public class ComponentFactory : IComponentFactory
    {
        public IGEComponent Create(string name)
        {
            IGEComponent component = null;
            switch (name)
            {
                case nameof(TransformComponent):
                    component = new TransformComponent();
                    break;
                case nameof(Sprite):
                    component = new Sprite();
                    break;
                case nameof(Animator):
                    component = new Animator();
                    break;
                case nameof(Animation):
                    component = new Animation();
                    break;
                default:
                    throw new Exception($"Unsupported component type! Type: {name}");
            }

            return component;
        }
    }
}
