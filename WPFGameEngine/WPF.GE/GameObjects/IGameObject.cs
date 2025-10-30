using System.Windows.Media;
using WPFGameEngine.Factories.Base;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Base;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public interface IGameObject : IGameEngineEntity
    {
        public int Id { get; }
        
        public double ZIndex { get; set; }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        public List<IGameObject> Children { get; }

        void Render(DrawingContext dc, Matrix parent);

        void Update(List<IGameObject> world, IGameTimer gameTimer);

        IGameObject RegisterComponent(IGEComponent component);

        IGameObject UnregisterComponent(IGEComponent component);

        IGameObject UnregisterComponent(string componentName);

        IGameObject UnregisterComponent<TComponent>()
            where TComponent : IGEComponent;

        void AddChild(IGameObject child);

        bool RemoveChild(Func<IGameObject, bool> predicate, bool recursive = false);
            
        IGameObject? FindChild(Func<IGameObject, bool> predicate, bool recursiveSearch = false);
        
        TComponent? GetComponent<TComponent>(bool throwException = true)
            where TComponent : IGEComponent;

        IEnumerable<IGEComponent> GetComponents();
    }
}
