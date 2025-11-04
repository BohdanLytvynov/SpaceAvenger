using WPFGameEngine.Factories.Base;
using WPFGameEngine.Factories.Components.Animations;
using WPFGameEngine.Factories.Components.Animators;
using WPFGameEngine.Factories.Components.RelativeTransforms;
using WPFGameEngine.Factories.Components.Sprites;
using WPFGameEngine.Factories.Components.Transform;
using WPFGameEngine.Factories.Ease.Linear;
using WPFGameEngine.Factories.Ease.Quadtratic;
using WPFGameEngine.Services.Interfaces;

namespace WPFGameEngine.FactoryWrapper.Base
{
    public interface IFactoryWrapper
    {
        IResourceLoader ResourceLoader { get; }
        IAnimationFactory AnimationFactory { get; }
        IAnimatorFactory AnimatorFactory { get; }
        ISpriteFactory SpriteFactory { get; }
        ITransformFactory TransformFactory { get; }
        IRelativeTransformFactory RelativeTransformFactory { get; }
        ILinearEaseFactory LinearEaseFactory { get; }
        IQuadraticEaseFactory QuadraticEaseFactory { get; }
        TType CreateObject<TType>() where TType : IGameEngineEntity;
        object CreateObject(string typeName);
    }
}
