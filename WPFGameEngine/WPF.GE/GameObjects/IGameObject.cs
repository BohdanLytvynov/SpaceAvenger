using System.Windows.Media.Imaging;
using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.AI.Base;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Dto.Base;
using WPFGameEngine.WPF.GE.Dto.GameObjects;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public interface IGameObject :
        IConvertToDto<GameObjectDto>, 
        IGameEngineEntity
    {
        #region AI
        IAIModule AIModule { get; }
        #endregion

        #region Metadata
        List<string> Metadata { get; }
        #endregion

        #region Lazy Loading
        IAnimation? Animation { get; }
        IAnimator? Animator { get; }
        ISprite? Sprite { get; }
        BitmapSource? Texture { get; }
        #endregion

        #region Main Game Object Properties
        public double ZIndex { get; set; }
        /// <summary>
        /// Enables Calculations for Update and Rendering
        /// </summary>
        public bool Enabled { get; set; }
        //Name of the Object that is used for mapping during object Creation in runtime
        //must be equal to the name of the GameObject in a game
        public string? ObjectName { get; set; }
        //Name or Id that can be used during game for searching objects
        public string? UniqueName { get; set; }
        public bool IsChild { get; }
        public List<IGameObject>? Children { get; }
        public IGameObject? Parent { get; set; }
        public int Id { get; }
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
        IEnumerable<TObject> GetAllChildrenOfType<TObject>(bool recursiveSearch = false)
            where TObject : class;
        IEnumerable<IGameObject> GetAllChildrenOfType(string typeName, bool recursiveSearch = false);
        #endregion
       
        #region Enable Disable Calculations
        void Enable(bool recursive = false);
        void Disable(bool recursive = false);
        bool IsEnabledAll(IGameObject gameObject);
        #endregion

        #region For Editor
        /// <summary>
        /// This is used in Editor to clear the Lazy Property if the component was removed
        /// </summary>
        void ForceUpdateOfLazyProperties();
        #endregion

    }
}
