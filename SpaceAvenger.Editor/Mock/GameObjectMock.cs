using System.Drawing;
using System.Numerics;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.Mock
{
    public class GameObjectMock : GameObject
    {
        public GameObjectMock(string name, Vector2 position, Vector2 centerPosition, double rotation, SizeF scale) 
            : base(name, position, centerPosition, rotation, scale)
        {
        }

        public GameObjectMock(string name) : base(name)
        {

        }

        public GameObjectMock() : base()
        {
            
        }
    }
}
