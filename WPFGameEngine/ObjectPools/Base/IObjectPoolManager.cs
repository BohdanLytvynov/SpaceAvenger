using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.ObjectPools.Base
{
    public interface IObjectPoolManager
    {
        void AddToPool(СacheableObject mapableObject);
        TObject? GetFromPool<TObject>()
            where TObject : СacheableObject;
    }
}
