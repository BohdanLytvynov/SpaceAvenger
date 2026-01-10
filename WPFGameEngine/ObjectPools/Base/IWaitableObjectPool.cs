using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.ObjectPools.Base
{
    public interface IWaitableObjectPool
    {
        void Update(double currentTime);

        void InsertWithDelay(СacheableObject obj, float delay);
    }
}
