using System.Drawing;
using System.Numerics;
using System.Security.Cryptography.Xml;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Dto.Base;
using WPFGameEngine.WPF.GE.Dto.GameObjects;
using WPFGameEngine.WPF.GE.Exceptions;
using WPFGameEngine.WPF.GE.Settings;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public abstract class GameObject : IGameObject
    {
        #region Nested Classes
        public class ZIndexGameObjectComparer : IComparer<IGameObject>
        {
            public int Compare(IGameObject? x, IGameObject? y) => x.ZIndex.CompareTo(y.ZIndex);
        }
        #endregion

        #region Fields
        private static int m_globId;

        private Dictionary<string, IGEComponent> m_components;
        private List<IGameObject> m_children;

        protected int m_id;
        #endregion

        #region Propeties
        public bool IsExported { get; set; }
        public bool Enabled { get; set; }
        public double ZIndex { get; set; }
        public string Name { get; set; }
        public List<IGameObject> Children { get => m_children; }
        public IGameObject Parent { get; set; }

        public bool IsChild 
        {
            get 
            {
                return Parent != null;
            }
        }
        #endregion

        #region Ctor
        public GameObject() : this(null)
        {
        }
        
        /// <summary>
        /// Main Ctor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <param name="centerPosition"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        public GameObject(string name)
        {
            Init();
            SetId();
            InitName(name);
        }

        #endregion

        #region Methods

        public virtual void Update(List<IGameObject> world, IGameTimer gameTimer)
        {
            if (!Enabled) return;
            Animation? animation = GetComponent<Animation>(false);
            Animator? animator = GetComponent<Animator>(false);

            if (animator != null)
            {
                animator.Update(gameTimer);
            }
            else if (animation != null)
            {
                animation.Update(gameTimer);
            }

            foreach (var child in m_children)
            {
                child.Update(world, gameTimer);
            }

            //Here must be a custom logic that must be implemented in Derived classes
        }

        public virtual void Render(DrawingContext dc, Matrix parent = default)
        {
            if (!Enabled) return;

            TransformComponent transform = GetTransformComponent();
            //Get An Image for Render
            BitmapSource bitmapSource = GetTexture();
            if (bitmapSource == null)
                return;
            //Calculate actual size of the Image
            float actualWidth = (float)bitmapSource.Width * transform.Scale.Width;
            float actualHeight = (float)bitmapSource.Height * transform.Scale.Height;

            //Calculate the center of the Image
            float Xcenter = actualWidth * transform.CenterPosition.X;
            float Ycenter = actualHeight * transform.CenterPosition.Y;
            //Get matrix for current game object
            
            if (IsChild)
            {

            }

            var globalMatrix = transform.GetLocalTransformMatrix(new Vector2(Xcenter, Ycenter));

            if (parent != default)
            {
                globalMatrix.Append(parent);
            }

            dc.PushTransform(new MatrixTransform(globalMatrix));

            dc.DrawImage(bitmapSource, new System.Windows.Rect
                (0, 0, actualWidth, actualHeight));

            if (GESettings.DrawGizmo)
            {
                //Draw Gizmo
                dc.DrawLine(
                    GESettings.XAxisColor,
                    new System.Windows.Point(Xcenter, Ycenter),
                    new System.Windows.Point(Xcenter + actualWidth * (1 - transform.CenterPosition.X), Ycenter));

                dc.DrawLine(
                    GESettings.YAxisColor,
                    new System.Windows.Point(Xcenter, Ycenter),
                    new System.Windows.Point(Xcenter, Ycenter + actualHeight * (1 - transform.CenterPosition.Y)));

                dc.DrawEllipse(
                    GESettings.GizmoCenterBrush,
                    GESettings.GizmoCenterPen,
                    new System.Windows.Point(Xcenter, Ycenter),
                    GESettings.GizmoCenterXRadius * transform.Scale.Width,
                    GESettings.GizmoCenterYRadius * transform.Scale.Height);
            }

            if (GESettings.DrawBorders)
            {
                dc.DrawRectangle(
                    GESettings.BorderRectangleBrush,
                    GESettings.BorderRectanglePen,
                    new System.Windows.Rect(
                        0, 0,
                        actualWidth,
                        actualHeight)
                    );
            }

            if (GESettings.DrawColliders)
            {

            }

            dc.Pop();

            foreach (var item in m_children)
            {
                item.Render(dc, globalMatrix);
            }
        }

        private void Init()
        {
            m_components = new Dictionary<string, IGEComponent>();
            m_children = new List<IGameObject>();
            Enabled = true;
            ZIndex = 0;
        }

        private void InitName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Name = this.GetType().Name + $"_{m_id}";
            }
            else
            {
                Name = name;
            }
        }

        private void SetId()
        {
            m_id = ++m_globId;
        }

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
            if (throwException && !m_components.ContainsKey(componentName))
                throw new ComponentNotFoundException(componentName);

            IGEComponent component = null;
            if (m_components.TryGetValue(componentName, out component))
            {
                return (TComponent)component;
            }
            return default;
        }

        public IEnumerable<IGEComponent> GetComponents()
        { 
            return m_components.Values;
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

                    if(res)
                        return;
                }
            }
        }

        public IGameObject UnregisterComponent<TComponent>() 
            where TComponent : IGEComponent
        {
            string name = typeof(TComponent).Name;

            if(m_components.ContainsKey(name))
                m_components.Remove(name);
            return this;
        }

        public void Traverse(IGameObject root, Action<IGameObject> traverseAction)
        { 
            if(root == null) 
                return;

            traverseAction?.Invoke(root);

            foreach (var item in root.Children)
            {
                Traverse(item, traverseAction);
            }
        }

        private GameObjectDto ToDtoRec(IGameObject root)
        {
            if (root == null)
                return null;

            if (!root.IsExported)
                return null;

            GameObjectDto gameObjectDto = new GameObjectDto();
            gameObjectDto.Name = root.Name;
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

        public void ClearAllComponents()
        {
            if(m_components.Count > 0)
                m_components.Clear();
        }

        public virtual void StartUp()
        {
            foreach (var item in m_children)
            {
                item.StartUp();
            }
        }

        private void ScaleRecursive(IGameObject obj, SizeF newScale)
        {
            if (obj == null)
                return;

            var t = obj.GetComponent<TransformComponent>();

            if (t != null && t.Scale != newScale)
            {
                t.Scale = newScale;
            }

            foreach (var item in obj.Children)
            {
                ScaleRecursive(item, newScale);
            }
        }

        public void Scale(SizeF newScale)
        {
            ScaleRecursive(this, newScale);
        }

        public BitmapSource GetTexture()
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

        public TransformComponent GetTransformComponent()
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

        #endregion
    }
}
