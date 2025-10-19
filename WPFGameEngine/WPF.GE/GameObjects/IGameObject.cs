using System.Drawing;
using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.WPF.GE.Component.Base;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public interface IGameObject 
    {
        public int Id { get; init; }

        public Vector2 Position { get; set; }

        public Vector2 CenterPosition { get; set; }

        public double Rotation { get; set; }

        public SizeF Scale { get; set; }

        public double ZIndex { get; set; }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        void Render(DrawingContext dc, Matrix parent);

        void Update(List<IGameObject> world);

        IGameObject RegisterComponent(IComponent component);

        IGameObject UnregisterComponent(IComponent component);

        void AddChild(GameObject child);

        void RemoveChild(GameObject child);

        Matrix GetGlobalTransformMatrix(Vector2 center);

        public TComponent? GetComponent<TComponent>(string componentName, bool throwException = true)
            where TComponent : IComponent;
    }
}
