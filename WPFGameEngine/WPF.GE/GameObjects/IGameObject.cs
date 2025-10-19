using System.Drawing;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Media;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Transforms;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public interface IGameObject 
    {
        public int Id { get; }
        
        public double ZIndex { get; set; }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        void Render(DrawingContext dc, Matrix parent);

        void Update(List<IGameObject> world);

        IGameObject RegisterComponent(IComponent component);

        IGameObject UnregisterComponent(IComponent component);

        void AddChild(GameObject child);

        void RemoveChild(GameObject child);

        public TComponent? GetComponent<TComponent>(bool throwException = true)
            where TComponent : IComponent;
    }
}
