using System.Drawing;
using System.Numerics;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.Mock
{
    public class GameObjectMock : GameObject, IGameObjectMock
    {
        public bool IsExported { get; set; }

        public GameObjectMock() : base()
        {
            
        }
    }
}
