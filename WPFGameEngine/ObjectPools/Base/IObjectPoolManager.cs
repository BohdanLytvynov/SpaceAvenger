using WPFGameEngine.ObjectPools.ThreadSafePools;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.ObjectPools.Base
{
    public interface IObjectPoolManager
    {
        /// <summary>
        /// Add an Object To Pool
        /// </summary>
        /// <param name="delayedItem">Object with Delayed Time</param>
        void AddToPool(DelayedItem delayedItem);
        /// <summary>
        /// Get an Object From Pool
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <returns>Object ready to use</returns>
        TObject? GetFromPool<TObject>()
            where TObject : СacheableObject;
        /// <summary>
        /// Get an Object From Pool
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns>Object ready to use</returns>
        СacheableObject? GetFromPool(string typeName);
        /// <summary>
        /// Clear all the existing Pools
        /// </summary>
        void Clear();
        /// <summary>
        /// Updates all the Object Pools global times
        /// </summary>
        /// <param name="globalTime"></param>
        void Update(double globalTime);
    }
}
