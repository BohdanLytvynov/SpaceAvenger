using System.Drawing;
using System.Numerics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Base;
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
        #endregion

        #region Ctor
        public GameObject() : this(null)
        {
        }

        public GameObject(string name) : this(name, new TransformComponent())
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
        public GameObject(string name, Vector2 position, Vector2 centerPosition, double rotation, SizeF scale)
        {
            Init();
            RegisterComponent(new TransformComponent(position, centerPosition, rotation, scale));
            SetId();
            InitName(name);
        }

        public GameObject(string name, ITransform transform)
        {
            Init();
            RegisterComponent(transform);
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
            TransformComponent transform = GetComponent<TransformComponent>();
            Sprite? sprite = GetComponent<Sprite>(false);
            Animation? animation = GetComponent<Animation>(false);
            Animator? animator = GetComponent<Animator>(false);
            //Get An Image for Render
            BitmapSource bitmapSource = null;
            if (animation != null && animation.Texture != null)
            {
                bitmapSource = animation.GetCurrentFrame();
            }
            else if (animator != null && animator.Current != null)
            {
                bitmapSource = animator.GetCurrentFrame();
            }
            else if(sprite != null)
            {
                bitmapSource = (BitmapSource)sprite.Texture;
            }

            if (bitmapSource == null)
                return;

            //Calculate the center of the Image
            float Xcenter = (float)(bitmapSource.Width * transform.Scale.Width) * transform.CenterPosition.X;
            float Ycenter = (float)(bitmapSource.Height * transform.Scale.Height) * transform.CenterPosition.Y;
            //Get matrix for current game object
            var globalMatrix = transform.GetLocalTransformMatrix(new Vector2(Xcenter, Ycenter));

            if (parent != default)
            {
                globalMatrix.Append(parent);
            }

            dc.PushTransform(new MatrixTransform(globalMatrix));

            dc.DrawImage(bitmapSource, new System.Windows.Rect(0, 0, bitmapSource.Width * transform.Scale.Width,
                bitmapSource.Height * transform.Scale.Height));

            if (GESettings.DrawGizmo)
            {
                //Draw Gizmo
                dc.DrawLine(
                    GESettings.XAxisColor,
                    new System.Windows.Point(Xcenter, Ycenter),
                    new System.Windows.Point(Xcenter + (bitmapSource.Width * transform.Scale.Width) * (1 - transform.CenterPosition.X), Ycenter));

                dc.DrawLine(
                    GESettings.YAxisColor,
                    new System.Windows.Point(Xcenter, Ycenter),
                    new System.Windows.Point(Xcenter, Ycenter + (bitmapSource.Height * transform.Scale.Height) * (1 - transform.CenterPosition.Y)));

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
                        bitmapSource.Width * transform.Scale.Width,
                        bitmapSource.Height * transform.Scale.Height)
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

        private void ToDtoRec(IGameObject root , GameObjectDto gameObjectDto)
        {
            if (root == null)
                return;

            if (!root.IsExported)
                return;

            gameObjectDto.Name = root.Name;
            gameObjectDto.Enabled = root.Enabled;
            gameObjectDto.ZIndex = root.ZIndex;
            var components = root.GetComponents();
            foreach (var component in components)
            {
                gameObjectDto.Components.Add(component.ToDto());
            }
            int i  = 0;
            foreach (var item in root.Children)
            {
                gameObjectDto.Children.Add(item.ToDto());

                ToDtoRec(item, gameObjectDto.Children[i]);
                i++;
            }
        }

        public GameObjectDto ToDto()
        {
            GameObjectDto gameObjectDto = new GameObjectDto();
            ToDtoRec(this, gameObjectDto);
            return  gameObjectDto;
        }

        #endregion
    }
}
