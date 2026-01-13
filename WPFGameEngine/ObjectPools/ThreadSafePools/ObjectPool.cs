using System.Collections.Concurrent;
using System.Diagnostics;
using WPFGameEngine.ObjectPools.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.ObjectPools.ThreadSafePools
{
    /// <summary>
    /// Struct that holds Object and Cooling Time
    /// </summary>
    public struct DelayedItem
    {
        /// <summary>
        /// Object
        /// </summary>
        public СacheableObject Cacheable { get; }
        /// <summary>
        /// Gets Time when the reset will be finished
        /// </summary>
        public double GetReadyAtTime(double globalTime) => Delay + globalTime;
        /// <summary>
        /// Delay Time
        /// </summary>
        public double Delay { get; set; }

        public DelayedItem(СacheableObject сacheableObject, double delay)
        {
            Cacheable = сacheableObject;
            Delay = delay;
        }
    }

    /// <summary>
    ///Thread Safe Object Pool with delay
    /// </summary>
    public class ObjectPool : IObjectPool
    {
        #region Fields
        private readonly ConcurrentStack<СacheableObject> m_AvailableStack;//Stack that holds objects that are available
        private readonly ConcurrentQueue<DelayedItem> m_waitingObjects;//Queue that holds objects that are in cooling stage

        #endregion

        #region Ctor
        public ObjectPool()
        {
            m_AvailableStack = new ConcurrentStack<СacheableObject>();
            m_waitingObjects = new ConcurrentQueue<DelayedItem>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Try to Get an object after it finished cooling procedure, also OnGetFromPool will be called
        /// </summary>
        /// <returns>Object or null</returns>
        public СacheableObject? Get()
        {
            if (m_AvailableStack.TryPop(out СacheableObject? obj))
            {
                obj.Enable(true);
                obj.Show();
                obj.OnGetFromPool();
                Debug.WriteLine($"{obj.ObjectName} taken from the Pool");
                return obj;
            }
            return null;
        }
        /// <summary>
        /// Inserts the Object with predefined delay
        /// </summary>
        /// <param name="delayedItem">Object for inserting</param>
        public void InsertWithDelay(DelayedItem delayedItem)
        {
            Debug.WriteLine($"{delayedItem.Cacheable.ObjectName} Added to Waiting queue");
            delayedItem.Cacheable.OnAddToPool();
            m_waitingObjects.Enqueue(delayedItem);
        }
        /// <summary>
        /// Is Poll enpty?
        /// </summary>
        /// <returns>Boolean 1 - empty, 0 - has some objects</returns>
        public bool IsEmpty()
        {
            return m_AvailableStack.IsEmpty;
        }
        /// <summary>
        /// Updates global game time, checks if some objects finishes cooling, moves them to the availables
        /// </summary>
        /// <param name="currentTime">Game global time</param>
        public void Update(double currentTime)
        {
            //Try to peek the first object from the queue, check if it is ready
            while (m_waitingObjects.TryPeek(out var delayedItem) && currentTime >= delayedItem.GetReadyAtTime(currentTime))
            {
                //Move object from the waiting queue to the avaliables
                if (m_waitingObjects.TryDequeue(out var readyItem))
                {
                    Debug.WriteLine($"{delayedItem.Cacheable.ObjectName} Added to Pool after waiting");
                    //Disable all the calculations for the Item that will be added to the pool
                    readyItem.Cacheable.Disable(true);
                    m_AvailableStack.Push(readyItem.Cacheable);
                }
            }
        }
        /// <summary>
        /// Clear Stack and Queue
        /// </summary>
        public void Clear()
        {
            m_AvailableStack.Clear();
            m_waitingObjects.Clear();
        }
        #endregion
    }
}
