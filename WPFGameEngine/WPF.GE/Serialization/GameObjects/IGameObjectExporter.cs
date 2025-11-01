using WPFGameEngine.WPF.GE.Dto.GameObjects;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Serialization.Base;

namespace WPFGameEngine.WPF.GE.Serialization.GameObjects
{
    public interface IGameObjectExporter : IObjectExporter<IGameObject, GameObjectDto>
    {
    }
}
