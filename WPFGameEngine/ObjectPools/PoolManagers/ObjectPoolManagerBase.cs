using WPFGameEngine.ObjectPools.Base;
using WPFGameEngine.ObjectPools.ThreadSafePools;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.ObjectPools.PoolManagers
{
    public abstract class ObjectPoolManagerBase<TObjectPool> : IObjectPoolManager
        where TObjectPool : class, IObjectPool, new()
    {
        protected Dictionary<string, TObjectPool> PoolManagerMap;
        protected ObjectPoolManagerBase()
        {
            PoolManagerMap = new Dictionary<string, TObjectPool>();
        }
        public abstract void AddToPool(DelayedItem delayedItem);
        public virtual void Clear()
        {
            foreach (var pool in PoolManagerMap.Values)
            { 
                pool.Clear();
            }

            PoolManagerMap.Clear();
        }
        public abstract TObject? GetFromPool<TObject>() where TObject : СacheableObject;
        public abstract СacheableObject? GetFromPool(string typeName);

        public void Update(double globalTime)
        {
            foreach (var pool in PoolManagerMap.Values)
            { 
                pool.Update(globalTime);
            }
        }
    }
}
