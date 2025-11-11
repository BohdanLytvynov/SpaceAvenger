using WPFGameEngine.ObjectPools.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.ObjectPools.SimpleObjectPools
{
    public class ObjectPool : IObjectPool
    {
        #region Fields
        private Queue<СacheableObject> m_objectPool;
        #endregion

        #region Ctor
        public ObjectPool()
        {
            m_objectPool = new Queue<СacheableObject>(10);
        }

        public СacheableObject? Get()
        {
            if(!IsEmpty())
                return m_objectPool.Dequeue();

            return null;
        }

        public void Insert(СacheableObject mapableObject)
        {
            m_objectPool.Enqueue(mapableObject);
        }

        /// <summary>
        /// Pool is empty? O(1)
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return m_objectPool.Count == 0;
        }
        #endregion
    }
}
