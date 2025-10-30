using System.Reflection;
using System.Xml.Linq;
using WPFGameEngine.Factories.Base;
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

        #region Fields
        private Dictionary<string, IFactory> m_ProductFactoryMap;
        #endregion

        #region Properties

        public IAnimationFactory AnimationFactory { get; init; }

        public IAnimatorFactory AnimatorFactory { get; init; }

        public ISpriteFactory SpriteFactory { get; init; }

        public ITransformFactory TransformFactory { get; init; }

        public ILinearEaseFactory LinearEaseFactory { get; init; }

        public IQuadraticEaseFactory QuadraticEaseFactory { get; init; }

        #region Add Tools
        public IResourceLoader ResourceLoader => m_resourceLoader;
        #endregion

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

            m_ProductFactoryMap = new Dictionary<string, IFactory>();

            var type = this.GetType();
            var factories = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var f in factories)
            {
                var obj = f.GetValue(this);
                if (obj is IFactory)
                {
                    IFactory factory = (IFactory)obj;
                    m_ProductFactoryMap.Add(factory.ProductName, factory);
                }
            }
        }

        public TType CreateObject<TType>() where TType : IGameEngineEntity
        {
            var name = typeof(TType).Name;
            return (TType)CreateObject(name);
        }

        public object CreateObject(string typeName)
        {
            return m_ProductFactoryMap[typeName].Create();
        }
        #endregion

    }
}
