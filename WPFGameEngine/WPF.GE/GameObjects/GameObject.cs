using System.Windows.Media.Imaging;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Dto.Base;
using WPFGameEngine.WPF.GE.Dto.GameObjects;
using WPFGameEngine.WPF.GE.Exceptions;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public abstract class GameObject : IGameObject
    {
        #region Metadata
        
        #endregion

        #region Nested Classes
        public class ZIndexGameObjectComparer : IComparer<IGameObject>
        {
            public int Compare(IGameObject? x, IGameObject? y) => x.ZIndex.CompareTo(y.ZIndex);
        }
        #endregion

        #region Fields
        private static int m_globId;

        private Dictionary<string, IGEComponent>? m_components;
        private List<IGameObject>? m_children;

        protected int m_id;

        #region Lazy Loading
        private IAnimation? m_animation;
        private IAnimator? m_animator;
        private ISprite? m_sprite;
        #endregion

        #endregion

        #region Propeties

        #region Lazy Loading

        public IAnimation? Animation 
        {
            get
            {
                if (m_animation == null)
                    m_animation = GetComponent<Animation>(false);
                return m_animation;
            }
        }
        public IAnimator? Animator 
        {
            get
            {
                if (m_animator == null)
                    m_animator = GetComponent<Animator>(false);
                return m_animator;
            }
        }
        public ISprite? Sprite 
        {
            get
            {
                if (m_sprite == null)
                    m_sprite = GetComponent<Sprite>(false);
                return m_sprite;
            }
        }
        public BitmapSource? Texture 
        {
            get; protected set;
        }
        #endregion

        public bool Enabled { get; set; }
        public double ZIndex { get; set; }
        public string? ObjectName { get; set; }
        public string? UniqueName { get; set; }
        public List<IGameObject>? Children { get => m_children; }
        public IGameObject? Parent { get; set; }
        public int Id { get => m_id; }

        public bool IsChild
        {
            get 
            {
                return Parent != null;
            }
        }

        public List<string> Metadata { get; }

        #endregion

        #region Ctor
        protected GameObject()
        {
            ObjectName = string.Empty;
            UniqueName = string.Empty;
            m_components = new Dictionary<string, IGEComponent>();
            m_children = new List<IGameObject>();
            Enabled = true;
            ZIndex = 0;
            m_id = ++m_globId;
            UniqueName = this.GetType().Name + $"_{m_id}";
            Metadata = new List<string>();
        }
        
        #endregion

        #region Methods

        #region Components
        /// <summary>
        /// Register a new Component
        /// </summary>
        /// <param name="component"></param>
        /// <returns>Game Object with a new registered component</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ComponentAlreadyRegisteredException"></exception>
        /// <exception cref="IncompatibleComponentException"></exception>
        public IGameObject RegisterComponent(IGEComponent component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            if (m_components.ContainsKey(component.ComponentName))
                throw new ComponentAlreadyRegisteredException(component.ComponentName);

            if (component.IncompatibleComponents != null)
            {
                var keyList = m_components.Keys.ToList();
                var validRes = component.IncompatibleComponents.Intersect(keyList).ToList();

                if (validRes.Count > 0)
                    throw new IncompatibleComponentException(component.ComponentName, validRes);
            }

            m_components.Add(component.ComponentName, component);

            return this;
        }

        public IGameObject UnregisterComponent(IGEComponent component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            if (m_components.ContainsKey(component.ComponentName))
                m_components.Remove(component.ComponentName);

            return this;
        }

        public IGameObject UnregisterComponent(string componentName)
        {
            if(string.IsNullOrEmpty(componentName))
                throw new ArgumentNullException(nameof(componentName));

            if (m_components.ContainsKey(componentName))
                m_components.Remove(componentName);

            return this;
        }

        public TComponent? GetComponent<TComponent>(bool throwException = true)
            where TComponent : IGEComponent
        {
            var componentName = typeof(TComponent).Name;
            return (TComponent)GetComponent(componentName, throwException);
        }

        /// <summary>
        /// Gets the component of the Game Object
        /// </summary>
        /// <param name="componentName"></param>
        /// <param name="throwException"></param>
        /// <returns>Components</returns>
        /// <exception cref="ComponentNotFoundException"></exception>
        public IGEComponent? GetComponent(string componentName, bool throwException = true)
        {
            if (throwException && !m_components.ContainsKey(componentName))
                throw new ComponentNotFoundException(componentName);

            IGEComponent component = null;
            if (m_components.TryGetValue(componentName, out component))
            {
                return component;
            }
            return component;
        }

        public IEnumerable<IGEComponent> GetComponents()
        { 
            return m_components.Values;
        }

        public IGameObject UnregisterComponent<TComponent>()
            where TComponent : IGEComponent
        {
            string name = typeof(TComponent).Name;

            if (m_components.ContainsKey(name))
                m_components.Remove(name);
            return this;
        }

        public void ClearAllComponents()
        {
            if (m_components.Count > 0)
                m_components.Clear();
        }

        #endregion

        #region Hierarchy

        public static void RemoveObject(Func<IGameObject, bool> predicate, List<IGameObject> world, bool recursive = false)
        {
            foreach (var o in world)
            {
                if (predicate(o))
                {
                    world.Remove(o);
                    return;
                }
                else if (recursive)
                {
                    bool res = o.RemoveChild(predicate, recursive);

                    if (res)
                        return;
                }
            }
        }

        public void AddChild(IGameObject child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            child.Parent = this;
            m_children.Add(child);
        }

        private void RemoveChildRecursive(List<IGameObject> children, Func<IGameObject, bool> predicate,
            ref bool removed)
        {
            if (removed)
                return;

            foreach (var item in children)
            {
                if (removed)
                    return;

                if (predicate(item))
                { 
                    m_children.Remove(item);
                    removed = true;
                    return;
                }

                RemoveChildRecursive(item.Children, predicate, ref removed);
            }
        }

        public bool RemoveChild(Func<IGameObject, bool> predicate, bool recursive = false)
        {
            if (!recursive)
            {
                foreach (var item in m_children)
                {
                    if (predicate(item))
                    {
                        m_children.Remove(item);
                        return true;
                    }
                }

                return false;
            }
            else
            {
                bool removed = false;
                RemoveChildRecursive(m_children, predicate, ref removed);
                return removed;
            }
        }

        private void GetChildRecursive(Func<IGameObject, bool> predicate, List<IGameObject> children, ref IGameObject result,
            bool found = false)
        {
            if (found)
                return;

            foreach (var item in m_children)
            {
                if (found)
                    return;

                if (predicate(item))
                { 
                    result = item;
                    found = true;
                    return;
                }

                GetChildRecursive(predicate, item.Children, ref result, found);
            }
        }

        public IGameObject? FindChild(Func<IGameObject, bool> predicate, bool recursiveSearch = false)
        {
            IGameObject result = null;

            if (!recursiveSearch)
            {
                foreach (var item in m_children)
                {
                    if (predicate(item))
                    {
                        result = item;
                        break;
                    }
                }
            }
            else
            {
                GetChildRecursive(predicate, m_children, ref result);
            }

            return result;
        }

        private void GetAllChildrenOfTypeRec(
            string typeName, IGameObject gameObject, List<IGameObject> result)
        {
            if (gameObject == null)
                return;

            foreach (var item in gameObject.Children)
            {
                if (item.ObjectName.Equals(typeName))
                { 
                    result.Add(item);
                }

                GetAllChildrenOfTypeRec(typeName, item, result);
            }
        }

        public IEnumerable<TObject> GetAllChildrenOfType<TObject>(bool recursiveSearch = false)
            where TObject : class
        {
            var r = GetAllChildrenOfType(typeof(TObject).Name, recursiveSearch);

            foreach (var item in r)
            {
                yield return (TObject)item;
            }

        }

        public IEnumerable<IGameObject> GetAllChildrenOfType(string typeName, bool recursiveSearch = false)
        { 
            List<IGameObject> result = new List<IGameObject>();

            if (!recursiveSearch)
            {
                foreach (var item in m_children)
                {
                    if (item.ObjectName?.Equals(typeName) ?? false)
                    {
                        result.Add(item);
                    }
                }
            }
            else
            {
                GetAllChildrenOfTypeRec(typeName, this, result);
            }

            return result;
        }

        public void Traverse(IGameObject root, Action<IGameObject> traverseAction)
        {
            if (root == null)
                return;

            traverseAction?.Invoke(root);

            foreach (var item in root.Children)
            {
                Traverse(item, traverseAction);
            }
        }

        #endregion

        #region ToDto

        private GameObjectDto ToDtoRec(IGameObject root)
        {
            if (root == null)
                return null;

            if (!(root as IExportable)?.IsExported ?? false)
                return null;

            GameObjectDto gameObjectDto = new GameObjectDto();
            gameObjectDto.ObjectName = root.ObjectName;
            gameObjectDto.UniqueName = root.UniqueName;
            gameObjectDto.Enabled = root.Enabled;
            gameObjectDto.ZIndex = root.ZIndex;
 
            var components = root.GetComponents();
            foreach (var component in components)
            {
                gameObjectDto.Components.Add((ComponentDto)component.ToDto());
            }

            foreach (var item in root.Children)
            {
                var childDto = ToDtoRec(item);
                gameObjectDto.Children.Add(childDto);
            }

            return gameObjectDto;
        }

        public GameObjectDto ToDto()
        {
            return ToDtoRec(this);
        }

        #endregion

        protected BitmapSource GetTexture()
        {
            Sprite? sprite = GetComponent<Sprite>(false);
            Animation? animation = GetComponent<Animation>(false);
            Animator? animator = GetComponent<Animator>(false);

            if (animation != null && animation.Texture != null)
            {
                return animation.GetCurrentFrame();
            }
            else if (animator != null && animator.Current != null)
            {
                return animator.GetCurrentFrame();
            }
            else if (sprite != null)
            {
                return (BitmapSource)sprite.Texture;
            }

            return null;
        }

        protected TransformComponent GetTransformComponent()
        {
            if (!IsChild)
            {
                return GetComponent<TransformComponent>(false);
            }
            else
            {
                return GetComponent<RelativeTransformComponent>(false);
            }
        }

        public void Enable(bool recursive = false)
        {
            Enabled = true;
            if (!recursive) return;
            foreach (var item in m_children)
            {
                item.Enable(recursive);
            }
        }

        public void Disable(bool recursive = false)
        {
            Enabled = false;
            if (!recursive) return;
            foreach (var item in m_children)
            {
                item.Disable(recursive);
            }
        }

        public bool IsEnabledAll(IGameObject gameObject)
        {
            if (gameObject == null)
            {
                return false;
            }

            if (!gameObject.Enabled)
            {
                return false;
            }

            foreach (var child in gameObject.Children)
            {
                bool childEnabled = IsEnabledAll(child);

                if (!childEnabled)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsEnabledAny(IGameObject gameObject)
        {
            if (gameObject == null)
            {
                return false;
            }

            if (gameObject.Enabled)
            {
                return true;
            }

            foreach (var child in gameObject.Children)
            {
                bool childEnabled = IsEnabledAny(child);

                if (childEnabled)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
