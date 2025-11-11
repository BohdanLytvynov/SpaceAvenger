using WPFGameEngine.ObjectPools.Base;
using WPFGameEngine.ObjectPools.SimpleObjectPools;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.ObjectPools.PoolManagers
{
    public class ObjectPoolManager : IObjectPoolManager
    {
        #region Fields
        private Dictionary<string, IObjectPool> m_PoolMap;
        #endregion

        #region Ctor
        public ObjectPoolManager()
        {
            m_PoolMap = new Dictionary<string, IObjectPool>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds object to the Pool B:O(1) W:O(n)
        /// </summary>
        /// <param name="mapableObject"></param>
        public void AddToPool(СacheableObject mapableObject)
        {
            string name = mapableObject.ObjectName;//O(1)

            if (!m_PoolMap.ContainsKey(name))//O(1)
            {
                m_PoolMap.Add(name, new ObjectPool());//O(1)
            }

            m_PoolMap[name].Insert(mapableObject);//O(1)
        }

        public TObject? GetFromPool<TObject>()
            where TObject : СacheableObject
        {
            IObjectPool pool = null;
            string name = typeof(TObject).Name;

            if (m_PoolMap.TryGetValue(name, out pool) && pool != null)
            {
                return pool.Get() as TObject;
            }

            return null;
        }
        #endregion
    }
}
