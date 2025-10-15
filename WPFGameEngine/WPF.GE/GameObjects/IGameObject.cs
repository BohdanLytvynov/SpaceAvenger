using System.Drawing;
using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Sprites;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public interface IGameObject 
    {
        public int Id { get; init; }

        public Vector2 Position { get; set; }

        public double Rotation { get; set; }

        public SizeF Scale { get; set; }

        public double ZIndex { get; set; }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        void Render(DrawingContext dc, Matrix parent);

        void Update(List<IGameObject> world);

        GameObject RegisterComponent(IComponent component);

        GameObject UnregisterComponent(IComponent component);

        void AddChild(GameObject child);

        void RemoveChild(GameObject child);

        Matrix GetGlobalTransformMatrix(Vector2 center);
    }
}
