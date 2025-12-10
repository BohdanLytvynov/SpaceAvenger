using WPFGameEngine.ObjectBuilders.Base;
using WPFGameEngine.ObjectPools.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.GameViewControl
{
    public interface IMapableObjectViewHost : IGameObjectViewHost
    {
        IObjectPoolManager ObjectPoolManager { get; init; }
        IObjectBuilder ObjectBuilder { get; init; }
        TObject Instantiate<TObject>(Action<IGameObject>? preStartUpConfig = null, 
            Action<IGameObject> postStartUpConfig = null,
            bool useCache = true)
            where TObject : СacheableObject;
        СacheableObject Instantiate(string typeName, Action<IGameObject>? preStartUpConfig = null,
            Action<IGameObject> postStartUpConfig = null, bool useCache = true);
    }
}
