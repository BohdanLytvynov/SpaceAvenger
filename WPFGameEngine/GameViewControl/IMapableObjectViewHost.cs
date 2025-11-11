using WPFGameEngine.ObjectBuilders.Base;
using WPFGameEngine.ObjectPools.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.GameViewControl
{
    public interface IMapableObjectViewHost : IGameObjectViewHost
    {
        IObjectPoolManager ObjectPoolManager { get; init; }
        IObjectBuilder ObjectBuilder { get; init; }
        TObject Instantinate<TObject>(bool useCache = true)
            where TObject : СacheableObject;
    }
}
