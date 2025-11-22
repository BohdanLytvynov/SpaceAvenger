using WPFGameEngine.WPF.GE.GameObjects.Collidable;

namespace SpaceAvenger.Editor.Mock
{
    public class GameObjectMock : CollidableBase, IGameObjectMock
    {
        public bool IsExported { get; set; }

        public GameObjectMock()
        {
            IsVisible = true;
        }
    }
}
