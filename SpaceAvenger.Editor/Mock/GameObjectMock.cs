using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.GameObjects.Collidable;

namespace SpaceAvenger.Editor.Mock
{
    public class GameObjectMock : CollidableBase, IGameObjectMock
    {
        public bool IsExported { get; set; }

        public GameObjectMock() : base()
        {
            IsVisible = true;
        }

        public override string ToString()
        {
            return $"Type: {ObjectName} UniqueName: {UniqueName}";
        }

        private IGameObjectMock CloneRec(IGameObjectMock src)
        { 
            if(src == null)
                return this;

            IGameObjectMock gameObject = new GameObjectMock()
            {
                Enabled = src.Enabled,
                ZIndex = src.ZIndex,
                ObjectName = src.ObjectName,
                UniqueName = src.UniqueName,
                IsVisible = src.IsVisible,
                IsExported = src.IsExported,
            };

            var components = src.GetComponents();

            foreach (var component in components)
            {
                gameObject.RegisterComponent((IGEComponent)component.Clone());
            }

            foreach (var child in src.Children)
            {
                gameObject.AddChild((IGameObjectMock)(child as IGameObjectMock).Clone());
            }

            return gameObject;

        }

        public object Clone()
        {
            return CloneRec(this);
        }
    }
}
