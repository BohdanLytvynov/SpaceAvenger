using WPFGameEngine.ObjectPools.ThreadSafePools;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.ObjectPools.PoolManagers
{
    public class ObjectPoolManager : ObjectPoolManagerBase<ObjectPool>
    {        
        #region Ctor
        public ObjectPoolManager() : base()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds object to the Pool B:O(1) W:O(n)
        /// </summary>
        /// <param name="mapableObject"></param>
        public override void AddToPool(DelayedItem delayedItem)
        {
            string name = delayedItem.Cacheable.ObjectName;//O(1)

            if (!PoolManagerMap.ContainsKey(name))//O(1)
            {
                PoolManagerMap.Add(name, new ObjectPool());//O(1)
            }

            PoolManagerMap[name].InsertWithDelay(delayedItem);//O(1)
        }

        public override TObject? GetFromPool<TObject>() where TObject : class
        {
            return GetFromPool(typeof(TObject).Name) as TObject;
        }

        public override СacheableObject? GetFromPool(string typeName)
        {
            ObjectPool? pool = null;
          
            if (PoolManagerMap.TryGetValue(typeName, out pool) && pool != null)
            {
                return pool.Get();
            }

            return null;
        }
        #endregion
    }
}
