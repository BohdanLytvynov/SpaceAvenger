using System.Reflection;
using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.ObjectBuilders.Base;
using WPFGameEngine.WPF.GE.Dto.GameObjects;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Serialization.GameObjects;

namespace WPFGameEngine.ObjectBuilders
{
    public class ObjectBuilder : IObjectBuilder
    {
        #region Fields

        private Assembly m_current;

        private Dictionary<string, Type> m_ObjectsForCreation;

        private Dictionary<string, GameObjectDto> m_ObjectDtos;

        private IGameObjectImporter m_objectImporter;

        private IFactoryWrapper m_factoryWrapper;

        #endregion

        #region Properties
        public IFactoryWrapper FactoryWrapper { get => m_factoryWrapper; }
        #endregion

        #region Ctor
        public ObjectBuilder(
            Assembly assembly,
            Type mapableType, 
            IGameObjectImporter gameObjectImporter,
            IFactoryWrapper factoryWrapper)
        {
            if(mapableType == null)
                throw new ArgumentNullException(nameof(mapableType));

            m_factoryWrapper = factoryWrapper ?? throw new ArgumentNullException(nameof(factoryWrapper));
            m_objectImporter = gameObjectImporter ?? throw new ArgumentNullException(nameof(gameObjectImporter));
            m_current = assembly ?? throw new ArgumentNullException(nameof(assembly));

            m_ObjectsForCreation = new Dictionary<string, Type>();
            
            var types = m_current.GetTypes().Where(t => mapableType.IsAssignableFrom(t)
            && !t.IsAbstract);

            foreach (var type in types)
            {
                m_ObjectsForCreation.Add(type.Name, type);
            }

            m_ObjectDtos = new Dictionary<string, GameObjectDto>();

            foreach (var objDto in m_objectImporter.ImportObjects())
            {
                m_ObjectDtos.Add(objDto.Name, objDto);
            }
        }
        #endregion

        #region Methods
        
        private MapableObject Map(GameObjectDto dto)
        {
            if (dto == null)
                return null;

            //1) Create an Object
            var mapable = CreateInstance(dto.Name);
            mapable.ClearAllComponents();
            foreach (var c in dto.Components)
            {
                mapable.RegisterComponent(c.ToObject(m_factoryWrapper));
            }

            foreach (var ch in dto.Children)
            { 
                var child = Map(ch);
                mapable.AddChild(child);
            }

            return mapable;
        }

        private GameObjectDto? GetDto(string objName)
        {
            if (string.IsNullOrEmpty(objName))
                throw new ArgumentNullException(nameof(objName));

            GameObjectDto? dto = null;

            if (!m_ObjectDtos.TryGetValue(objName, out dto))
            {
                throw new InvalidOperationException($"{objName} does not exist in dtos dictionary!");
            }

            return dto;
        }

        private MapableObject CreateInstance(string objName)
        {
            Type type = null;
            GameObjectDto dto = null;
            if (!m_ObjectsForCreation.TryGetValue(objName, out type))
            {
                throw new InvalidOperationException($"{objName} was not found in Assembly: {m_current.FullName}");
            }
            
            return (MapableObject)Activator.CreateInstance(type);
        }

        public MapableObject Build(string objName)
        {
            var dto = GetDto(objName);

            return Map(dto);
        }

        public TObject Build<TObject>() 
            where TObject : MapableObject
        { 
            var name = typeof(TObject).Name;

            return (TObject)Build(name);
        }


        #endregion
    }
}
