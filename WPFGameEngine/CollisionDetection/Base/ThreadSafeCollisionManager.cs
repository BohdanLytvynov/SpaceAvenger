using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.CollisionDetection.Base
{
    //Not used
    public abstract class ThreadSafeCollisionManager<TCollisionData, TCollisionObject> : 
        IThreadSafeCollisionManager<TCollisionData>
        where TCollisionData : new()
    {
        #region Fields
        //used when we make a world snapshot
        protected readonly object m_worldLock;
        // Sync object, is used when we SWAP buffer's references
        protected readonly object m_swapLock;
        //Is collision detection task running, volitile means that CPU won't cash this value
        protected volatile bool m_running;
        //Cancels collision task
        protected CancellationTokenSource m_cancellationTokenSource;
        //Actual task that checks collisions
        protected Task m_checkTask;
        //Objects that are filtered from the World that can collide
        protected List<TCollisionObject> m_currentCollidableObjects;
        //Copy of the world
        protected List<IGameObject> m_worldSnapshot;
        // Collection to be read by the Game Loop, when we want to get collision info lists
        protected Dictionary<int, List<TCollisionData>> m_ReadOnlyBuffer;
        // Collection, that is filled by the collision checker thread
        protected Dictionary<int, List<TCollisionData>> m_BackBuffer;
        // Pool of Lists<CollisionData> used to reduce the amount of  the allocations
        protected readonly Stack<List<TCollisionData>> m_ListPool;
        //Reference to the world object that Stores all the object in a Scene, Is set outside
        public List<IGameObject> World { get; set; }
        #endregion

        #region Ctor
        public ThreadSafeCollisionManager()
        {
            m_ReadOnlyBuffer = new Dictionary<int, List<TCollisionData>>();
            m_BackBuffer = new Dictionary<int, List<TCollisionData>>();
            m_currentCollidableObjects = new List<TCollisionObject>(128);
            m_worldSnapshot = new List<IGameObject>(256);
            m_ListPool = new Stack<List<TCollisionData>>();
            m_worldLock = new object();
            m_swapLock = new object();
            m_running = false;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Start the collision check
        /// </summary>
        public virtual void Start()
        {
            if (!m_running)
            {
                m_cancellationTokenSource = new CancellationTokenSource();
                m_checkTask = Task.Run(() => CheckCollisions(m_cancellationTokenSource.Token));
                m_running = true;
            }
        }
        /// <summary>
        /// Pause the collision check
        /// </summary>
        public virtual void Pause()
        {
            if (m_running)
                m_running = false;
        }
        /// <summary>
        /// Stop the collision check. When we exit from the Level or the Game
        /// </summary>
        public virtual void Stop()
        {
            if (m_running)
            {
                m_cancellationTokenSource.Cancel();//Request collision task cancellation

                try
                {
                    m_checkTask.Wait();//Wait until task will stop
                }
                catch (System.AggregateException)
                {

                }
                //Clear the state of the collision System
                Clear();
                m_checkTask = null;
                m_running = false;
            }
        }
        /// <summary>
        //Adds an object to the backbuffer using List<CollisionData> Pool
        /// </summary>
        /// <param name="ownerId">An objects</param>
        /// <param name="data">List of all the CollisionData with objects that collided with an Object</param>
        protected virtual void AddToBackBuffer(int ownerId, TCollisionData data)
        {
            if (!m_BackBuffer.TryGetValue(ownerId, out var list))
            {
                list = GetListFromPool();//Use Pool instead fo new Allocations, it will be clean
                m_BackBuffer.Add(ownerId, list);
            }
            list.Add(data);
        }

        /// <summary>
        /// Reads the content of the readonly buffer, This is called from the Game Loop
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual List<TCollisionData> GetCollisionInfo(int key)
        {
            lock (m_swapLock)//Lock the Front Buffer for reading it
            {
                if (m_ReadOnlyBuffer.TryGetValue(key, out var info))
                {
                    return info;
                }
                return null;
            }
        }
        /// <summary>
        /// Main Collision checker
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected virtual async Task CheckCollisions(CancellationToken token) { }
        
        /// <summary>
        /// Resume the collision checking
        /// </summary>
        public virtual void Resume()
        {
            if (!m_running)
                m_running = true;
        }
        /// <summary>
        /// Clear the Collision Checker, is called when we exit the level or the game
        /// </summary>
        public virtual void Clear()
        {
            lock (m_swapLock)
            {
                // 1. Clean the buffers
                m_ReadOnlyBuffer.Clear();
                m_BackBuffer.Clear();
                // Clean the lists in a Pool
                foreach (var list in m_ListPool)
                {
                    list.Clear();
                }
                //Clean the filtered objects
                m_currentCollidableObjects.Clear();
                //Clean the pool
                m_ListPool.Clear();
            }
            //We'll lock it to be sure that no external threads want to read data from it
            lock (m_worldLock)
            {
                //Clean world snapshot
                m_worldSnapshot.Clear();
            }
        }

        /// <summary>
        /// Used to remove an object by it's id from the Buffers, to be sure that it wan't be sent twice to the Game Loop
        /// </summary>
        /// <param name="id"></param>
        public virtual void ForceRemove(int id)
        {
            lock (m_swapLock)//Lock Buffers
            {
                // Remove from Front Buffer, that is used by the game loop
                m_ReadOnlyBuffer.Remove(id);

                // Remove from the back buffer and add the List<CollisionData> to the Pool
                if (m_BackBuffer.TryGetValue(id, out var list))
                {
                    m_ListPool.Push(list); //Return to the pool
                    m_BackBuffer.Remove(id);//Remove data
                }
            }
        }

        /// <summary>
        /// Return the empty ready to use list from pool, or create and return new one, if pool is empty
        /// </summary>
        /// <returns></returns>
        protected List<TCollisionData> GetListFromPool()
        {
            if (m_ListPool.Count > 0)
            {
                var list = m_ListPool.Pop();//Take a list from pool
                list.Clear();//Prepare it for use, clear from the previous data
                return list;
            }
            return new List<TCollisionData>(8);//If we have an empty pool, create the new list with some allocations
        }

        /// <summary>
        /// Clears the BackBuffer by pushing all it's List<CollisionData> to the List Pool
        /// </summary>
        protected virtual void PrepareBackBuffer()
        {
            foreach (var pair in m_BackBuffer)
            {
                m_ListPool.Push(pair.Value);
            }
            m_BackBuffer.Clear();
        }

        /// <summary>
        /// Swaps references of Readonly Buffer and BackBuffer
        /// </summary>
        protected virtual void SwapBuffers()
        {
            //Here we do lock and simple swap of references via temp
            lock (m_swapLock)
            {
                var temp = m_ReadOnlyBuffer;
                m_ReadOnlyBuffer = m_BackBuffer;
                m_BackBuffer = temp;
            }
        }
        #endregion
    }
}
