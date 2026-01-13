using WPFGameEngine.ObjectBuilders.Base;
using WPFGameEngine.ObjectPools.Base;
using WPFGameEngine.ObjectPools.ThreadSafePools;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.ObjectInstantiators
{
    public class ObjectInstantiator : IObjectInstantiator
    {
        IObjectBuilder m_builder;
        IObjectPoolManager m_poolManager;

        public ObjectInstantiator(IObjectBuilder objectBuilder, 
            IObjectPoolManager objectPoolManager)
        {
            m_builder = objectBuilder ?? throw new ArgumentNullException(nameof(objectBuilder));
            m_poolManager = objectPoolManager ?? throw new ArgumentNullException(nameof(objectPoolManager));
        }

        public void AddToPool(DelayedItem delayedItem)
        {
            m_poolManager.AddToPool(delayedItem);
        }

        public void Clear()
        {
            m_poolManager.Clear();
        }

        public TObject? Instantiate<TObject>(
            out bool poolUsed,
            bool useCache = true) where TObject : MapableObject
        {
            return Instantiate(typeof(TObject).Name, out poolUsed, useCache) as TObject;
        }

        public MapableObject? Instantiate(
            string typeName, out bool poolUsed,
            bool useCache = true)
        {
            СacheableObject? obj = null;
            poolUsed = false;
            if (!useCache)
            {
                obj = m_builder.Build(typeName) as СacheableObject;
            }
            else
            {
                obj = m_poolManager.GetFromPool(typeName);

                if (obj == null)
                {
                    obj = m_builder.Build(typeName) as СacheableObject;
                }
                else
                { 
                    poolUsed = true;
                }
            }
            return obj;
        }

        public void Update(double globalGameTime)
        {
            m_poolManager.Update(globalGameTime);
        }
    }
}
