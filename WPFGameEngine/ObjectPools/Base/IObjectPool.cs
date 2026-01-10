using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.ObjectPools.Base
{
    public interface IObjectPool
    {
        bool IsEmpty();

        СacheableObject? Get();

        void Insert(СacheableObject cachableObject);
    }
}
