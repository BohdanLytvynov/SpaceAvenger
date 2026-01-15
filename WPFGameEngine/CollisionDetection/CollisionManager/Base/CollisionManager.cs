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
        public ICollidable Object { get; }
        public Vector2 MTV { get; }
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
        private readonly object m_worldLock;

        private volatile bool m_running;
        private CancellationTokenSource m_cancellationTokenSource;
        private Task m_checkTask;
        List<ICollidable> m_currentObjects = new List<ICollidable>(128);
        private List<IGameObject> m_worldSnapshot = new List<IGameObject>(256);

        // "Чистовик" для игрового потока
        private Dictionary<int, List<CollisionData>> m_ReadOnlyBuffer = new();
        // "Черновик" для фонового потока
        private Dictionary<int, List<CollisionData>> m_BackBuffer = new();
        // Объект синхронизации только для момента подмены буферов
        private readonly object m_swapLock = new object();
        private readonly Stack<List<CollisionData>> m_ListPool = new Stack<List<CollisionData>>();

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
        public void Start()
        {
            if (!m_running)
            {
                m_cancellationTokenSource = new CancellationTokenSource();
                m_checkTask = Task.Run(() => CheckCollisions(m_cancellationTokenSource.Token));
                m_running = true;
            }
        }

        public void Pause()
        {
            if (m_running)
                m_running = false;
        }

        public void Stop()
        {
            if (m_running)
            {
                m_cancellationTokenSource.Cancel();

                try
                {
                    m_checkTask.Wait();
                }
                catch (System.AggregateException)
                {

                }
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
                list = GetListFromPool();//Use Pool instead fo new Allocations
                m_BackBuffer.Add(ownerId, list);
            }
            list.Add(data);
        }

        /// <summary>
        /// Reads the content of the readonly buffer
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<CollisionData>? GetCollisionInfo(int key)
        {
            lock (m_swapLock)
            {
                if (m_ReadOnlyBuffer.TryGetValue(key, out var info))
                {
                    return info;
                }
                return null;
            }
        }

        private async Task CheckCollisions(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                //Waiting if pause requested
                if (!m_running)
                {
                    try
                    {
                        await Task.Delay(100, token);
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                    continue;
                }

                lock (m_worldLock)
                {
                    m_worldSnapshot.Clear();
                    if (World != null)
                        m_worldSnapshot.AddRange(World);
                }

                m_currentObjects.Clear();
                //Good approach is to use froeach iterator instead of LINQs. 
                //Cause we should avoid lots Allocations, and GC procedures
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

                Debug.WriteLine($"{m_currentObjects.Count} Found that can Collide!");

                int len = m_currentObjects.Count;
                //Brute Force
                for (int i = 0; i < len; i++)//O(N^2)/2
                {
                    for (int j = i + 1; j < len; j++)
                    {
                        var obj1 = m_currentObjects[i];
                        var obj2 = m_currentObjects[j];

                        if (obj1.Id >= obj2.Id)
                            continue;

                        if (!CollisionMatrix.CanCollide(obj1.CollisionLayer, obj2.CollisionLayer))
                            continue;

                        var colInfo = CollisionHelper.Intersects(
                            obj1.Collider.CollisionShape,
                            obj2.Collider.CollisionShape);

                        if (colInfo.Intersects)
                        {
                            AddToBackBuffer(obj1.Id, new CollisionData(obj2, colInfo.MTV, colInfo.Overlap));
                            AddToBackBuffer(obj2.Id, new CollisionData(obj1, colInfo.MTV * -1, colInfo.Overlap));
                        }
                    }
                }

                SwapBuffers();
                PrepareBackBuffer();
                try
                {
                    await Task.Delay(CollisionSettings.CollisionCheckDelay_MS, token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        public void Resume()
        {
            if (!m_running)
                m_running = true;
        }

        public void Clear()
        {
            lock (m_swapLock)
            {
                // 1. Очищаем основные данные
                m_ReadOnlyBuffer.Clear();
                m_BackBuffer.Clear();
                // 2. Очищаем пул списков, чтобы освободить память в куче
                // Это важно при выходе, чтобы ссылки на ICollidable внутри списков не висели в памяти
                foreach (var list in m_ListPool)
                {
                    list.Clear();
                }
                m_currentObjects.Clear();
                m_ListPool.Clear();
            }
        }


        public void ForceRemove(int id)
        {
            lock (m_swapLock)
            {
                // Удаляем из текущего чистовика, чтобы логика Update его больше не видела
                m_ReadOnlyBuffer.Remove(id);

                // Удаляем из черновика, чтобы он не перекочевал в следующий кадр
                if (m_BackBuffer.TryGetValue(id, out var list))
                {
                    m_ListPool.Push(list); // Возвращаем список в пул
                    m_BackBuffer.Remove(id);
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
                var list = m_ListPool.Pop();
                list.Clear();
                return list;
            }
            return new List<CollisionData>(8);
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