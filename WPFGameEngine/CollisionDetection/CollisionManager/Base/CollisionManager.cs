using System.Numerics;
using WPFGameEngine.CollisionDetection.CollisionMatrixes;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.GameObjects.Collidable;
using WPFGameEngine.WPF.GE.Helpers;

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
        private readonly object m_lock;

        private volatile bool m_running;
        private CancellationTokenSource m_cancellationTokenSource;
        private Task m_checkTask;
        private const int COLLISION_CHECK_DELAY_MS = 16;

        private readonly Dictionary<int, List<CollisionData>> m_CollisionBuffer;

        public List<IGameObject> World { get; set; }
        #endregion

        #region Ctor
        public CollisionManager()
        {
            m_lock = new object();
            m_CollisionBuffer = new Dictionary<int, List<CollisionData>>();
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

        private void AddToBuffer(int ownerId, CollisionData collisionData)
        {
            // ownerId — это ID того, КТО столкнулся
            // collisionData.Object — это ТОТ, С КЕМ столкнулись
            if (m_CollisionBuffer.TryGetValue(ownerId, out var info))
            {
                info.Add(collisionData);
            }
            else
            {
                m_CollisionBuffer.Add(ownerId, new List<CollisionData>() { collisionData });
            }
        }

        public void RemoveFromBuffer(int key)
        {
            lock (m_lock)
            {
                if (!m_CollisionBuffer.ContainsKey(key)) return;

                m_CollisionBuffer.Remove(key);
            }
        }

        public List<CollisionData>? GetCollisionInfo(int key)
        {
            lock (m_lock)
            {
                if (m_CollisionBuffer.TryGetValue(key, out var info))
                {
                    return info;
                }
                return null;
            }
        }

        private async Task CheckCollisions(CancellationToken token)
        {
            //Init capacity is used to reduce amount of allocations
            List<ICollidable> currentObjects = new List<ICollidable>(128);

            while (!token.IsCancellationRequested)
            {
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

                lock (m_lock)
                {
                    currentObjects.Clear();
                    m_CollisionBuffer.Clear();
                    //Good approach is to use froeach iterator instead of LINQs. 
                    //Cause we should avoid lots Allocations, and GC procedures
                    foreach (var obj in World)
                    {
                        if (obj is ICollidable collidable &&
                            obj.Enabled && collidable.IsVisible &&
                            collidable.IsCollidable &&
                            collidable.Collider.CollisionEnabled &&
                            collidable.Collider.CollisionResolved)
                        {
                            currentObjects.Add(collidable);
                        }
                    }
                }

                int len = currentObjects.Count;
                //Brute Force
                for (int i = 0; i < len; i++)//O(N^2)/2
                {
                    for (int j = i + 1; j < len; j++)
                    {
                        var obj1 = currentObjects[i];
                        var obj2 = currentObjects[j];

                        if (obj1 == null || obj2 == null)
                            continue;

                        if(!CollisionMatrix.CanCollide(obj1.CollisionLayer, obj2.CollisionLayer))
                            continue;

                        var colInfo = CollisionHelper.Intersects(
                            obj1.Collider.CollisionShape,
                            obj2.Collider.CollisionShape);

                        if (colInfo.Intersects)
                        {
                            lock (m_lock)
                            {
                                // Для первого объекта записываем данные о втором
                                AddToBuffer(obj1.Id, new CollisionData(obj2, colInfo.MTV, colInfo.Overlap));
                                // Для второго объекта записываем данные о первом (инвертируем MTV)
                                AddToBuffer(obj2.Id, new CollisionData(obj1, colInfo.MTV * -1, colInfo.Overlap));
                            }
                        }
                    }
                }

                try
                {
                    await Task.Delay(COLLISION_CHECK_DELAY_MS, token);
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
            lock (m_lock)
            {
                m_CollisionBuffer.Clear();
            }
        }

        #endregion
    }
}