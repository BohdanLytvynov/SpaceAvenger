using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.Spaceships
{
    public class SpaceshipMock : GameObject
    {
        public SpaceshipMock(Vector2 position, double rotation, SizeF scale) 
            : base("Mock", position, rotation, scale)
        {
            Init();
        }

        public SpaceshipMock() : base("Mock")
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
