using SpaceAvenger.Editor.Mock;
using SpaceAvenger.Editor.ViewModels.Components.Animations;
using SpaceAvenger.Editor.ViewModels.Components.Animators;
using SpaceAvenger.Editor.ViewModels.Components.Base;
using SpaceAvenger.Editor.ViewModels.Components.Collider;
using SpaceAvenger.Editor.ViewModels.Components.RelativeTransforms;
using SpaceAvenger.Editor.ViewModels.Components.Sprites;
using SpaceAvenger.Editor.ViewModels.Components.Transform;
using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Collider;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Component.Transforms;

namespace SpaceAvenger.Editor.ViewModels.Helpers
{
    internal static class ComponentViewModelHelper
    {
        public static ComponentViewModel CreateComponentViewModel(
            IGEComponent component, IGameObjectMock gameObject,
            IFactoryWrapper factoryWrapper, IAssemblyLoader assemblyLoader)
        {
            ComponentViewModel c = null;
            switch (component.ComponentName)
            {
                case nameof(TransformComponent):
                    c = new TransformComponentViewModel(gameObject);
                    break;
                case nameof(Animation):
                    c = new AnimationComponentViewModel(gameObject,
                        factoryWrapper,
                        assemblyLoader);
                    break;
                case nameof(Animator):
                    c = new AnimatorComponentViewModel(gameObject,
                        assemblyLoader, factoryWrapper);
                    break;
                case nameof(Sprite):
                    c = new SpriteComponentViewModel(gameObject,
                        factoryWrapper.ResourceLoader);
                    break;
                case nameof(RelativeTransformComponent):
                    c = new RelativeTransformViewModel(gameObject);
                    break;
                case nameof(ColliderComponent):
                    c = new ColliderComponentViewModel(gameObject,
                        assemblyLoader,
                        factoryWrapper);
                    break;
                case nameof(RaycastComponent):
                    c = new RaycastComponentViewModel(gameObject);
                    break;
                default:
                    throw new Exception($"Unsupported component Type! Component: {component.ComponentName}");
            }
            return c;
        }
    }
}
