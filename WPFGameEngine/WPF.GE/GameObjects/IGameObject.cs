using System.Numerics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using WPFGameEngine.Factories.Base;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Collider;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Dto.Base;
using WPFGameEngine.WPF.GE.Dto.GameObjects;
using WPFGameEngine.WPF.GE.Math.Matrixes;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public interface IGameObject : IGameEngineEntity, IConvertToDto<GameObjectDto>
    {
        #region Lazy Loading
        ITransform Transform { get; }
        IAnimation Animation { get; }
        IAnimator Animator { get; }
        ISprite Sprite { get; }
        ICollider Collider { get; }
        public BitmapSource Texture { get; }
        #endregion

        #region Main Game Object Properties
        public bool IsCollidable { get; }
        /// <summary>
        /// Use for editor only not for games
        /// </summary>
        public bool IsExported { get; set; }
        public double ZIndex { get; set; }
        public bool Enabled { get; set; }
        //Name of the Object that is used for mapping during object Creation in runtime
        //must be equal to the name of the GameObject in a game
        public string ObjectName { get; set; }
        //Name or Id that can be used during game for searching objects
        public string UniqueName { get; set; }
        public bool IsChild { get; }
        public List<IGameObject> Children { get; }
        public IGameObject Parent { get; set; }
        public int Id { get; }
        #endregion

        #region Game Loop
        void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer);
        void Render(DrawingContext dc, Matrix3x3 parent);
        void Update();
        void ProcessCollision(CollisionInfo? collisionInfo);
        #endregion

        #region Components
        IGameObject RegisterComponent(IGEComponent component);
        IGameObject UnregisterComponent(IGEComponent component);
        IGameObject UnregisterComponent(string componentName);
        IGameObject UnregisterComponent<TComponent>()
            where TComponent : IGEComponent;
        TComponent? GetComponent<TComponent>(bool throwException = true)
            where TComponent : IGEComponent;
        IEnumerable<IGEComponent> GetComponents();
        IGEComponent? GetComponent(string componentName, bool throwException = true);
        /// <summary>
        /// Clear all the Components O(C)
        /// </summary>
        void ClearAllComponents();
        #endregion

        #region Hirarchy
        void AddChild(IGameObject child);
        bool RemoveChild(Func<IGameObject, bool> predicate, bool recursive = false);
        IGameObject? FindChild(Func<IGameObject, bool> predicate, bool recursiveSearch = false);
        #endregion

        #region Transform
        void Translate(Vector2 position);
        void Translate(Vector2 dir, float speed, double deltaTime);
        /// <summary>
        /// Rotates an object
        /// </summary>
        /// <param name="angle">In Degrees</param>
        void Rotate(double angle);
        void Scale(Size newScale);
        Matrix3x3 GetGlobalTransformMatrix();
        /// <summary>
        /// Returns Center of the object in world Coordinates
        /// </summary>
        /// <returns></returns>
        Vector2 GetWorldCenter();
        void LookAt(Vector2 position, double rotSpeed, double deltaTime);
        Vector2 GetDirection(Vector2 position);
        Size GetActualSize();
        #endregion

        #region Enable Disable
        void Enable(bool recursive = false);
        void Disable(bool recursive = false);
        bool IsEnabledAll(IGameObject gameObject);
        #endregion

    }
}
