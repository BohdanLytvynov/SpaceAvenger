using System.Collections.Concurrent;
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
        public СacheableObject Cacheable { get; set; }
        /// <summary>
        /// Time when the reset will be finished
        /// </summary>
        public double ReadyAt { get; set;  }
    }

    /// <summary>
    ///Thread Safe Object Pool with delay
    /// </summary>
    public class ThreadSafeObjectPool : IObjectPool, IWaitableObjectPool
    {
        #region Fields
        private double m_currentTime;//Current game time
        private readonly ConcurrentStack<СacheableObject> m_AvailableStack;//Stack that holds objects that are available
        private readonly ConcurrentQueue<DelayedItem> m_waitingObjects;//Queue that holds objects that are in cooling stage

        #endregion

        #region Ctor
        public ThreadSafeObjectPool()
        {
            m_AvailableStack = new ConcurrentStack<СacheableObject>();
            m_waitingObjects = new ConcurrentQueue<DelayedItem>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Try to Get an object after it finished cooling procedure
        /// </summary>
        /// <returns>Object or null</returns>
        public СacheableObject? Get()
        {
            if (m_AvailableStack.TryPop(out СacheableObject? obj))
            {
                obj.OnGetFromPool();
                return obj;
            }
            return null;
        }
        /// <summary>
        /// Inserts the Object with predefined delay
        /// </summary>
        /// <param name="obj">Object for inserting</param>
        /// <param name="delay">Time that must pass before object will be ready again to use</param>
        public void InsertWithDelay(СacheableObject obj, float delay)
        {
            obj.OnAddToPool();

            m_waitingObjects.Enqueue(new DelayedItem()
            {
                Cacheable = obj,
                ReadyAt = m_currentTime + delay
            });
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
            //Updating global time
            m_currentTime = currentTime;
            //Try to peek the first object from the queue, check if it is ready
            while (m_waitingObjects.TryPeek(out var delayedItem) && m_currentTime >= delayedItem.ReadyAt)
            {
                //Move object from the waiting queue to the avaliables
                if (m_waitingObjects.TryDequeue(out var readyItem))
                {
                    m_AvailableStack.Push(readyItem.Cacheable);
                }
            }
        }

        void IObjectPool.Insert(СacheableObject cachableObject)
        {
            throw new NotImplementedException("Can't be used in a Waitable Object Pool!");
        }

        #endregion
    }
}
