using System.Drawing;
using System.Numerics;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.Spaceships
{
    public class ModuleMock : GameObject
    {
        public ModuleMock(string name, Vector2 position, double rotation, SizeF scale) 
            : base(name, position, rotation, scale)
        {
            Init();
        }

        public ModuleMock(string name) : base(name)
        {
            Init();
        }

        private void Init()
        { 
            Sprite sprite = new Sprite();
            RegisterComponent(sprite);
        }
    }
}
