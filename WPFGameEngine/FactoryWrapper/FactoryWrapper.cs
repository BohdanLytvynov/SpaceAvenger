using System.Reflection;
using WPFGameEngine.Attributes.Factories;
using WPFGameEngine.Enums;
using WPFGameEngine.Extensions;
using WPFGameEngine.Factories.Base;
using WPFGameEngine.Factories.Components.Animations;
using WPFGameEngine.Factories.Components.Animators;
using WPFGameEngine.Factories.Components.Colliders;
using WPFGameEngine.Factories.Components.RelativeTransforms;
using WPFGameEngine.Factories.Components.Sprites;
using WPFGameEngine.Factories.Components.Transform;
using WPFGameEngine.Factories.Ease.Base;
using WPFGameEngine.Factories.Geometry.Base;
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
        public Dictionary<string, IFactory> ProductFactoryMap { get; protected set; }

        private IAnimationFactory m_AnimationFactory;
        private IAnimatorFactory m_AnimatorFactory;
        private ISpriteFactory m_SpriteFactory;
        private ITransformFactory m_TransformFactory;
        private IRelativeTransformFactory m_RelativeTransformFactory;
        private IColliderFactory m_colliderFactory;

        #endregion
        #region Add Tools
        public IResourceLoader ResourceLoader => m_resourceLoader;
        #endregion

        #region Ctor
        public FactoryWrapper(IResourceLoader resourceLoader)
        {
            m_resourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));

            m_AnimationFactory = new AnimationFactory(m_resourceLoader);
            m_AnimatorFactory = new AnimatorFactory();
            m_SpriteFactory = new SpriteFactory(m_resourceLoader);
            m_TransformFactory = new TransformFactory();
            m_RelativeTransformFactory = new RelativeTransformFactory();
            m_colliderFactory = new ColliderFactory();

            ProductFactoryMap = new Dictionary<string, IFactory>();

            var type = this.GetType();
            var factories = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            //Get Factories
            foreach (var f in factories)
            {
                var obj = f.GetValue(this);
                if (obj is IFactory)
                {
                    IFactory factory = (IFactory)obj;
                    ProductFactoryMap.Add(factory.ProductName, factory);
                }
            }

            var currAssembly = Assembly.GetAssembly(type);
            if (currAssembly != null)
            {
                //Get all ease types
                var easeTypes = currAssembly.GetTypes().Where(
                    t=> t.GetAttribute<BuildWithFactory<GEObjectType>>() != null
                    && t.GetAttribute<BuildWithFactory<GEObjectType>>()
                    .GetValue<GEObjectType>("GameObjectType") == GEObjectType.Ease);

                Type genericFactoryImpl = typeof(EaseFactoryBase<>);

                foreach (var easeType in easeTypes)
                {
                    Type closedFactoryType = genericFactoryImpl.MakeGenericType(easeType);

                    IFactory factory = (IFactory)Activator.CreateInstance(closedFactoryType);

                    ProductFactoryMap.Add(factory.ProductName, factory);
                }

                //Get all geometry types
                var geomTypes = currAssembly.GetTypes().Where(
                    t => t.GetAttribute<BuildWithFactory<GEObjectType>>() != null
                    && t.GetAttribute<BuildWithFactory<GEObjectType>>()
                    .GetValue<GEObjectType>("GameObjectType") == GEObjectType.Geometry);

                Type geomFactoryImpl = typeof(GeometryFactoryBase<>);

                foreach (var geomType in geomTypes)
                {
                    Type closedGeomFactoryType = geomFactoryImpl.MakeGenericType(geomType);

                    IFactory factory = (IFactory)Activator.CreateInstance(closedGeomFactoryType);

                    ProductFactoryMap.Add(factory.ProductName, factory);
                }
            }
        }

        #endregion

        public TType CreateObject<TType>() where TType : IGameEngineEntity
        {
            var name = typeof(TType).Name;
            return (TType)CreateObject(name);
        }

        public object CreateObject(string typeName)
        {
            return ProductFactoryMap[typeName].Create();
        }
        

    }
}
