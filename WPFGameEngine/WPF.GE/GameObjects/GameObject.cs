using System.Numerics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFGameEngine.Extensions;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Collider;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Dto.Base;
using WPFGameEngine.WPF.GE.Dto.GameObjects;
using WPFGameEngine.WPF.GE.Exceptions;
using WPFGameEngine.WPF.GE.Math.Basis;
using WPFGameEngine.WPF.GE.Math.Matrixes;
using WPFGameEngine.WPF.GE.Math.Sizes;
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

        #region Lazy Loading
        private ICollider m_colliderComponent;
        private ITransform m_transform;
        private IAnimation m_animation;
        private IAnimator m_animator;
        private ISprite m_sprite;
        #endregion

        #endregion

        #region Propeties

        public IGameObjectViewHost GameView { get; private set; }
        public IGameTimer GameTimer { get; private set; }

        #region Lazy Loading
        public ICollider Collider 
        { get
            {
                if (m_colliderComponent == null)
                    m_colliderComponent = GetComponent<ColliderComponent>(false);
                return m_colliderComponent;
            }
        }
        public ITransform Transform 
        {
            get
            { 
                if(m_transform == null)
                    m_transform = GetTransformComponent();
                return m_transform;
            }
        }
        public IAnimation Animation 
        {
            get
            {
                if (m_animation == null)
                    m_animation = GetComponent<Animation>(false);
                return m_animation;
            }
        }
        public IAnimator Animator 
        {
            get
            {
                if (m_animator == null)
                    m_animator = GetComponent<Animator>(false);
                return m_animator;
            }
        }
        public ISprite Sprite 
        {
            get
            {
                if (m_sprite == null)
                    m_sprite = GetComponent<Sprite>(false);
                return m_sprite;
            }
        }
        public BitmapSource Texture 
        {
            get; protected set;
        }
        #endregion

        public bool IsExported { get; set; }
        public bool IsVisible { get; set; }
        public bool Enabled { get; set; }
        public double ZIndex { get; set; }
        public string ObjectName { get; set; }
        public string UniqueName { get; set; }
        public List<IGameObject> Children { get => m_children; }
        public IGameObject Parent { get; set; }
        public int Id { get => m_id; }

        public bool IsChild
        {
            get 
            {
                return Parent != null;
            }
        }

        public bool IsCollidable => Collider != null;
        #endregion

        #region Ctor
        public GameObject() : this(null)
        {
        }
        
        /// <summary>
        /// Main Ctor
        /// </summary>
        /// <param name="uniqueName"></param>
        /// <param name="position"></param>
        /// <param name="centerPosition"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        public GameObject(string uniqueName)
        {
            Init();
            SetId();
            InitName(uniqueName);
        }

        #endregion

        #region Methods
        public virtual void Update()
        {
            if (!Enabled) return;

            Texture = GetTexture();

            if (Texture == null) return;

            //Calculate actual size of the Image
            float actualWidth = (float)Texture.Width * Transform.Scale.Width;
            float actualHeight = (float)Texture.Height * Transform.Scale.Height;

            //Negative Value Protection
            actualWidth = actualWidth < 0 ? 0 : actualWidth;
            actualHeight = actualHeight < 0 ? 0 : actualHeight;

            Transform.ActualSize = new Size(actualWidth, actualHeight);
            //Calculate the center of the Image
            float Xcenter = actualWidth * Transform.CenterPosition.X;
            float Ycenter = actualHeight * Transform.CenterPosition.Y;

            Transform.ActualCenterPosition = new Vector2(Xcenter, Ycenter);
            //Get matrix for current game object

            if (IsCollidable && Collider.CollisionShape != null)
            {
                Collider.ActualObjectSize = Transform.ActualSize;
                var globMatrix = GetGlobalTransformMatrix();
                var leftUpperCorner = globMatrix.GetTranslate();
                Collider.CollisionShape.Scale = Transform.Scale;
                Collider.CollisionShape.Scale.CheckNegativeSize();
                Collider.Basis = globMatrix.GetBasis();
                Collider.CollisionShape.Basis = Collider.Basis;
                Collider.CollisionShape.CenterPosition = leftUpperCorner +
                    Collider.ActualCenterPosition;
                Collider.CollisionShape.CalculatePoints();
            }

            if (Animator != null)
            {
                Animator.Update(GameTimer);
            }
            else if (Animation != null)
            {
                Animation.Update(GameTimer);
            }

            foreach (var child in m_children)
            {
                child.Update();
            }

            //Here must be a custom logic that must be implemented in Derived classes
        }
        //Need to move code that is dependent on System.Windows.Media
        //And all related classes like Bitmapsource, DrawingContext, Matrix, SizeF,
        //GetLocalTransform matrix
        public virtual void Render(DrawingContext dc, Matrix3x3 parent)
        {
            if (!Enabled) return;

            if (!IsVisible) return;

            if (Texture == null) return;

            var actualWidth = Transform.ActualSize.Width;
            var actualHeight = Transform.ActualSize.Height;
            var globalMatrix = Transform.GetLocalTransformMatrix();

            if (parent != Matrix3x3.Identity)
            {
                globalMatrix *= parent;
            }

            Matrix m = new Matrix();
            m.M11 = globalMatrix.M11;
            m.M12 = globalMatrix.M12;
            m.M21 = globalMatrix.M21;
            m.M22 = globalMatrix.M22;
            m.OffsetX = globalMatrix.OffsetX; 
            m.OffsetY = globalMatrix.OffsetY;

            dc.PushTransform(new MatrixTransform(m));

            var Xcenter = Transform.ActualCenterPosition.X;
            var Ycenter = Transform.ActualCenterPosition.Y;

            dc.DrawImage(Texture, new System.Windows.Rect
                (0, 0, actualWidth, actualHeight));

            if (GESettings.DrawGizmo)
            {
                //Draw Gizmo
                dc.DrawLine(
                    GESettings.XAxisColor,
                    new System.Windows.Point(Xcenter, Ycenter),
                    new System.Windows.Point(Xcenter + actualWidth * (1 - Transform.CenterPosition.X), Ycenter));

                dc.DrawLine(
                    GESettings.YAxisColor,
                    new System.Windows.Point(Xcenter, Ycenter),
                    new System.Windows.Point(Xcenter, Ycenter + actualHeight * (1 - Transform.CenterPosition.Y)));

                dc.DrawEllipse(
                    GESettings.GizmoCenterBrush,
                    GESettings.GizmoCenterPen,
                    new System.Windows.Point(Xcenter, Ycenter),
                    GESettings.GizmoCenterXRadius * Transform.Scale.Width,
                    GESettings.GizmoCenterYRadius * Transform.Scale.Height);
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

            dc.Pop();

            if (GESettings.DrawColliders 
                && IsCollidable 
                && Collider.CollisionShape != null)
            {
                Collider.CollisionShape.Render(dc);
            }

            foreach (var item in m_children)
            {
                var childTransform = item.Transform as IRelativeTransform;
                if (childTransform != null)
                {
                    childTransform.ActualParentSize = new Size(actualWidth, actualHeight);
                }
                item.Render(dc, globalMatrix);
            }
        }

        private void Init()
        {
            IsVisible = true;
            ObjectName = string.Empty;
            UniqueName = string.Empty;
            m_components = new Dictionary<string, IGEComponent>();
            m_children = new List<IGameObject>();
            Enabled = true;
            ZIndex = 0;
        }

        private void InitName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                UniqueName = this.GetType().Name + $"_{m_id}";
            }
            else
            {
                UniqueName = name;
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
            return (TComponent)GetComponent(componentName, throwException);
        }

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
                    if (item.ObjectName.Equals(typeName))
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

        public void ClearAllComponents()
        {
            if(m_components.Count > 0)
                m_components.Clear();
        }

        public virtual void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            GameView = viewHost;
            GameTimer = gameTimer;

            Texture = GetTexture();

            foreach (var item in m_children)
            {
                item.StartUp(viewHost, gameTimer);
            }
        }

        private void ScaleRecursive(IGameObject obj, Size newScale)
        {
            if (obj == null)
                return;

            var t = obj.Transform;

            if (t != null && t.Scale != newScale)
            {
                t.Scale = new Size(newScale.Width + t.Scale.Width, 
                    newScale.Height + t.Scale.Height);
            }

            foreach (var item in obj.Children)
            {
                ScaleRecursive(item, newScale);
            }
        }

        public void Scale(Size newScale)
        {
            float dx = newScale.Width - Transform.Scale.Width;
            float dy = newScale.Height - Transform.Scale.Height;
            ScaleRecursive(this, new Size(dx, dy));
        }

        private BitmapSource GetTexture()
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

        private TransformComponent GetTransformComponent()
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

        public void Translate(Vector2 position)
        {
            Transform.Position = position;
        }

        public void Translate(Vector2 dir, float speed, double deltaTime)
        {
            Translate(
                Transform.Position + 
                dir.Multiply(speed * deltaTime));
        }

        public void Rotate(double angle)
        { 
            Transform.Rotation = angle;
        }

        public Matrix3x3 GetGlobalTransformMatrix()
        {
            Matrix3x3 m = Matrix3x3.Identity;
            GetGlobalMatrixRec(this, ref m);
            return m;
        }
        #region Static Methods
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

        private static void GetGlobalMatrixRec(IGameObject obj, ref Matrix3x3 matrix)
        { 
            if(obj == null)
                return;

            var t = obj.Transform;
            if (t != null)
            {
                var m = t.GetLocalTransformMatrix();
                matrix *= m;

                GetGlobalMatrixRec(obj.Parent, ref matrix);
            }
        }

        public static Matrix3x3 GetGlobalTransformMatrix(IGameObject obj)
        {
            Matrix3x3 m = new Matrix3x3();
            GetGlobalMatrixRec(obj, ref m);
            return m;
        }

        #endregion

        public Vector2 GetWorldCenter()
        {
            var m = this.GetGlobalTransformMatrix();
            var b = m.GetBasis();
            var center = Transform.ActualCenterPosition;
            var lx = b.X.Multiply(center.X);
            var ly = b.Y.Multiply(center.Y);
            var l = lx + ly;
            return m.GetTranslate() + l;
        }

        public Vector2 GetDirection(Vector2 position)
        {
            var objW = GetWorldCenter();
            return Vector2.Normalize(position - objW);
        }

        public Basis2D GetBasis()
        { 
            return GetGlobalTransformMatrix().GetBasis();
        }

        public void LookAt(Vector2 position, double rotSpeed, double deltaTime)
        {
            //Get Dir Vector to Target
            var dir = GetDirection(position);
            //Check that we are not in bounds of target
            if (dir.LengthSquared() < 0.0001)
                return;
            //Get Local Basis -> Xl, Yl
            var basis = GetBasis();
            //Calculate angle to rotate to
            var angle = dir.GetAngleDeg(basis.X);
            //Decide the hemicircle of rotation according to Yl
            double sign = Vector2.Dot(dir, basis.Y) < 0 ? -1 : 1;
            //Get Current Angle
            double currAngle = Transform.Rotation;
            //Calculate destination angle
            double destAngle = currAngle + sign*angle;
            //LERP
            //Get short rotation Way
            double diff = destAngle - currAngle;
            //Clamp between -180, 180, so we can operate using hemicirles
            while (diff > 180) diff -= 360;
            while (diff < -180) diff += 360;
            //Claculate rotation Step independent to FPS
            double step = diff * rotSpeed * deltaTime;
            //Get new Angle
            double newAngle = currAngle + step;
            //Apply new rotation
            Rotate(newAngle);
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

        public Size GetActualSize()
        {
            var transform = Transform;
            return new Size((float)Texture.Width * transform.Scale.Width,
                (float)Texture.Height * transform.Scale.Height);
        }

        public virtual void ProcessCollision(List<IGameObject>? collisionInfo)
        {
            
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

        public void Hide()
        {
            IsVisible = false;
        }

        public void Show()
        {
            IsVisible = true;
        }

        #endregion
    }
}
