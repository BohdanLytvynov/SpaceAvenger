using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFGameEngine.Factories.Base;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Dto.Base;
using WPFGameEngine.WPF.GE.Dto.GameObjects;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public interface IGameObject : IGameEngineEntity, IConvertToDto<GameObjectDto>
    {
        /// <summary>
        /// Use for editor only not for games, Unique key must be Name for games
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
        BitmapSource GetTexture();
        TransformComponent GetTransformComponent();
        void StartUp();
        void Render(DrawingContext dc, Matrix parent);
        void Update(List<IGameObject> world, IGameTimer gameTimer);
        IGameObject RegisterComponent(IGEComponent component);
        IGameObject UnregisterComponent(IGEComponent component);
        IGameObject UnregisterComponent(string componentName);
        IGameObject UnregisterComponent<TComponent>()
            where TComponent : IGEComponent;
        void ClearAllComponents();
        void AddChild(IGameObject child);
        bool RemoveChild(Func<IGameObject, bool> predicate, bool recursive = false);
        IGameObject? FindChild(Func<IGameObject, bool> predicate, bool recursiveSearch = false);
        TComponent? GetComponent<TComponent>(bool throwException = true)
            where TComponent : IGEComponent;
        IEnumerable<IGEComponent> GetComponents();

        void Scale(SizeF newScale);
    }
}
