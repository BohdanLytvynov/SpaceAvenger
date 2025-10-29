using System.Reflection;
using WPFGameEngine.Factories.Components.Animations;
using WPFGameEngine.Factories.Components.Animators;
using WPFGameEngine.Factories.Components.Sprites;
using WPFGameEngine.Factories.Components.Transform;
using WPFGameEngine.Factories.Ease;
using WPFGameEngine.Factories.Ease.Linear;
using WPFGameEngine.Factories.Ease.Quadtratic;
using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.Services.Interfaces;

namespace WPFGameEngine.FactoryWrapper
{
    public class FactoryWrapper : IFactoryWrapper
    {

        #region Add Tools
        IResourceLoader m_resourceLoader;
        #endregion

        #region Properties

        public IAnimationFactory AnimationFactory { get; init; }

        public IAnimatorFactory AnimatorFactory { get; init; }

        public ISpriteFactory SpriteFactory { get; init; }

        public ITransformFactory TransformFactory { get; init; }

        public ILinearEaseFactory LinearEaseFactory { get; init; }

        public IQuadraticEaseFactory QuadraticEaseFactory { get; init; }

        public IResourceLoader ResourceLoader => m_resourceLoader;

        #endregion

        #region Ctor
        public FactoryWrapper(IResourceLoader resourceLoader)
        {
            m_resourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));

            AnimationFactory = new AnimationFactory(m_resourceLoader);
            AnimatorFactory = new AnimatorFactory();
            SpriteFactory = new SpriteFactory(m_resourceLoader);
            TransformFactory = new TransformFactory();
            LinearEaseFactory = new LinearEaseFactory();
            QuadraticEaseFactory = new QuadraticEaseFactory();

            var type = this.GetType();
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            { 
                
            }
        }

        public TType CreateObject<TType>()
        {
            return default;
        }

        public object CreateObject(string typeName)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
