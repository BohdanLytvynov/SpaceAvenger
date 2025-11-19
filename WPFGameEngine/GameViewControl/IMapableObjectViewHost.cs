using System.Windows.Media.TextFormatting;
using WPFGameEngine.ObjectBuilders.Base;
using WPFGameEngine.ObjectPools.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.GameViewControl
{
    public interface IMapableObjectViewHost : IGameObjectViewHost
    {
        IObjectPoolManager ObjectPoolManager { get; init; }
        IObjectBuilder ObjectBuilder { get; init; }
        TObject Instantiate<TObject>(Action<IGameObject>? config = null, bool useCache = true)
            where TObject : СacheableObject;
        СacheableObject Instantiate(string typeName, Action<IGameObject>? config = null, bool useCache = true);
    }
}
