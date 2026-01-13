using WPFGameEngine.ObjectPools.ThreadSafePools;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.ObjectInstantiators
{
    public interface IObjectInstantiator
    {
        /// <summary>
        /// Creates an Object and adds it to the Object Pool
        /// </summary>
        /// <typeparam name="TObject">Type of the object for creation</typeparam>
        /// <param name="useCache">Do we need to put object into the Pull?</param>
        /// <returns>Cretaed object</returns>
        TObject? Instantiate<TObject>(
            out bool poolUsed,
            bool useCache = true)
            where TObject : MapableObject;
        /// <summary>
        /// Creates an Object and adds it to the Object Pool
        /// </summary>
        /// <param name="typeName">Name of the Type for Creation</param>
        /// <param name="useCache">Do we need to put object into the Pull?</param>
        /// <returns></returns>
        MapableObject? Instantiate(
            string typeName, out bool poolUsed,
            bool useCache = true);

        /// <summary>
        /// Updates global time in a PoolManager
        /// </summary>
        /// <param name="globalGameTime"></param>
        void Update(double globalGameTime);

        /// <summary>
        /// Clears All Object Pools 
        /// </summary>
        void Clear();
        /// <summary>
        /// Returns Object Back To the Object Pool
        /// </summary>
        /// <param name="delayedItem"></param>
        void AddToPool(DelayedItem delayedItem);
    }
}
