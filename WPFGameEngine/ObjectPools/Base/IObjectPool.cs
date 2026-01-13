using WPFGameEngine.ObjectPools.ThreadSafePools;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.ObjectPools.Base
{
    public interface IObjectPool
    {
        /// <summary>
        /// Is Poll enpty?
        /// </summary>
        /// <returns>Boolean 1 - empty, 0 - has some objects</returns>
        bool IsEmpty();
        /// <summary>
        /// Try to Get an object after it finished cooling procedure
        /// </summary>
        /// <returns>Object or null</returns>
        СacheableObject? Get();
        /// <summary>
        /// Updates global game time, checks if some objects finishes cooling, moves them to the availables
        /// </summary>
        /// <param name="currentTime">Game global time</param>
        void Update(double currentTime);
        /// <summary>
        /// Inserts the Object with predefined delay
        /// </summary>
        /// <param name="obj">Object for inserting</param>
        /// <param name="delay">Time that must pass before object will be ready again to use</param>
        void InsertWithDelay(DelayedItem delayedItem);
        /// <summary>
        /// Clears Object Pool
        /// </summary>
        void Clear();
    }
}
