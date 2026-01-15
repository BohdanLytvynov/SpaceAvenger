using System.Diagnostics;
using System.Numerics;
using WPFGameEngine.CollisionDetection.CollisionMatrixes;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.GameObjects.Collidable;
using WPFGameEngine.WPF.GE.Helpers;
using WPFGameEngine.WPF.GE.Settings;

namespace WPFGameEngine.CollisionDetection.CollisionManager.Base
{
    public struct CollisionData
    {
        /// <summary>
        /// Objects that is in collision 
        /// </summary>
        public ICollidable Object { get; }
        /// <summary>
        /// Minimum Translation Vector
        /// </summary>
        public Vector2 MTV { get; }
        /// <summary>
        /// How far overlapping occures
        /// </summary>
        public float Overlap { get; }

        public CollisionData(ICollidable gameObject, Vector2 mtv, float overlap)
        {
            Object = gameObject;
            MTV = mtv;
            Overlap = overlap;
        }
    }

    public class CollisionManager : ICollisionManager
    {
        #region Fields
        //used when we make a world snapshot
        private readonly object m_worldLock;
        //Is collision detection task running, volitile means that CPU won't cash this value
        private volatile bool m_running;
        //Cancels collision task
        private CancellationTokenSource m_cancellationTokenSource;
        //Actual task that checks collisions
        private Task m_checkTask;
        //Objects that are filtered from the World that can collide
        private List<ICollidable> m_currentObjects = new List<ICollidable>(128);
        //Copy of the world
        private List<IGameObject> m_worldSnapshot = new List<IGameObject>(256);
        // Collection to be read by the Game Loop, when we want to get collision info lists
        private Dictionary<int, List<CollisionData>> m_ReadOnlyBuffer = new();
        // Collection, that is filled by the collision checker thread
        private Dictionary<int, List<CollisionData>> m_BackBuffer = new();
        // Sync object, is used when we SWAP buffer's references
        private readonly object m_swapLock = new object();
        // Pool of Lists<CollisionData> used to reduce the amount of  the allocations
        private readonly Stack<List<CollisionData>> m_ListPool = new Stack<List<CollisionData>>();
        //Reference to the world object that Stores all the object in a Scene, Is set outside
        public List<IGameObject> World { get; set; }
        #endregion

        #region Ctor
        public CollisionManager()
        {
            m_ReadOnlyBuffer = new Dictionary<int, List<CollisionData>>();
            m_BackBuffer = new Dictionary<int, List<CollisionData>>();
            m_worldLock = new object();
            m_running = false;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Start the collision check
        /// </summary>
        public void Start()
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
        public void Pause()
        {
            if (m_running)
                m_running = false;
        }
        /// <summary>
        /// Stop the collision check. When we exit from the Level or the Game
        /// </summary>
        public void Stop()
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
        private void AddToBackBuffer(int ownerId, CollisionData data)
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
        public List<CollisionData>? GetCollisionInfo(int key)
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
        private async Task CheckCollisions(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                //Waiting if pause requested
                if (!m_running)
                {
                    try
                    {
                        await Task.Delay(100, token);//Wait for 100 ms, that is done to avoid the empty executions,
                        //Also it can react to cancellation scenario, and it will just cancel thread execution
                    }
                    catch (TaskCanceledException)
                    {
                        break;//In case of cancel scenario
                    }
                    continue;//Case if there
                }
                //Lock the access to world, to be sure that it can't be modified by another Thread, adding or removing of new object
                lock (m_worldLock)
                {
                    m_worldSnapshot.Clear();//Clear previous data
                    if (World != null)
                        m_worldSnapshot.AddRange(World);//Copy only references to objects
                }

                m_currentObjects.Clear();//Clear before filtration
                //Good approach is to use froeach iterator instead of LINQs. 
                //Cause we should avoid lots of Allocations, and GC procedures
                //Filter all the objects, that can collide
                foreach (var obj in m_worldSnapshot)
                {
                    if (obj is ICollidable collidable &&
                        obj.Enabled && collidable.IsVisible &&
                        collidable.IsCollidable &&
                        collidable.Collider.CollisionEnabled)
                    {
                        m_currentObjects.Add(collidable);
                    }
                }

                int len = m_currentObjects.Count;//Length of the collection with objects, that can collide, we reduce calls to the Count property
                //Brute Force
                for (int i = 0; i < len; i++)//O(N^2)/2
                {
                    for (int j = i + 1; j < len; j++)
                    {
                        var obj1 = m_currentObjects[i];
                        var obj2 = m_currentObjects[j];

                        if (obj1.Id >= obj2.Id)//Avoiding of double check A - B and B - A
                            continue;
                        //Check if that objects should Collide
                        if (!CollisionMatrix.CanCollide(obj1.CollisionLayer, obj2.CollisionLayer))
                            continue;
                        //Collision Checking
                        var colInfo = CollisionHelper.Intersects(
                            obj1.Collider.CollisionShape,
                            obj2.Collider.CollisionShape);

                        if (colInfo.Intersects)//Case of collision
                        {
                            //Write new data to the Back Buffer, it is used only by collision checking thread
                            AddToBackBuffer(obj1.Id, new CollisionData(obj2, colInfo.MTV, colInfo.Overlap));
                            AddToBackBuffer(obj2.Id, new CollisionData(obj1, colInfo.MTV * -1, colInfo.Overlap));
                        }
                    }
                }
                //Swap the references in Buffers, now Readonly Game - Loop buffer will be the Back Buffer,
                //where are the actual collision info is now placed
                SwapBuffers();
                //Prepare Back Buffer for next iteration
                PrepareBackBuffer();

                try
                {
                    //We use this to have some delay, 16 ms by default,
                    //again it can handle task cancellation scenario
                    await Task.Delay(CollisionSettings.CollisionCheckDelay_MS, token);
                }
                catch (TaskCanceledException)
                {
                    break;//Case of task cancellation
                }
            }
        }
        /// <summary>
        /// Resume the collision checking
        /// </summary>
        public void Resume()
        {
            if (!m_running)
                m_running = true;
        }
        /// <summary>
        /// Clear the Collision Checker, is called when we exit the level or the game
        /// </summary>
        public void Clear()
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
                m_currentObjects.Clear();
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
        public void ForceRemove(int id)
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
        private List<CollisionData> GetListFromPool()
        {
            if (m_ListPool.Count > 0)
            {
                var list = m_ListPool.Pop();//Take a list from pool
                list.Clear();//Prepare it for use, clear from the previous data
                return list;
            }
            return new List<CollisionData>(8);//If we have an empty pool, create the new list with some allocations
        }

        /// <summary>
        /// Clears the BackBuffer by pushing all it's List<CollisionData> to the List Pool
        /// </summary>
        private void PrepareBackBuffer()
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
        private void SwapBuffers()
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