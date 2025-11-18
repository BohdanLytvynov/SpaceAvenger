using System.Diagnostics;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Helpers;

namespace WPFGameEngine.CollisionDetection.CollisionManager.Base
{
    public class CollisionManager : ICollisionManager
    {
        #region Fields
        private readonly object m_lock;
        
        private volatile bool m_running;
        private CancellationTokenSource m_cancellationTokenSource;
        private Task m_checkTask;
        private const int COLLISION_CHECK_DELAY_MS = 16;

        private readonly Dictionary<int, CollisionInfo> m_CollisionBuffer;

        public List<IGameObject> World { get; set; }
        #endregion

        #region Ctor
        public CollisionManager()
        {
            m_lock = new object();
            m_CollisionBuffer = new Dictionary<int, CollisionInfo>();
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

        private void AddToBuffer(int key, IGameObject gameObject)
        {
            if (m_CollisionBuffer.TryGetValue(key, out var info))
            {
                info.Add(gameObject);
            }
            else
            {
                var colInfo = new CollisionInfo();
                colInfo.Add(gameObject);
                m_CollisionBuffer.Add(key, colInfo);
            }
        }

        public void RemoveFromBuffer(int key)
        {
            lock (m_lock)
            {
                if (!m_CollisionBuffer.ContainsKey(key)) return;
                if (!m_CollisionBuffer[key].Resolved) return;
                m_CollisionBuffer.Remove(key);
            }
        }

        public CollisionInfo? GetCollisionInfo(int key)
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

                List<IGameObject> currentObjects;
                lock (m_lock)
                {
                    currentObjects = World.Where(x => x != null && x.Enabled &&
                    x.IsCollidable && 
                    x.Collider.CollisionEnabled &&
                    x.Collider.CollisionResolved).ToList();
                }

                int len = currentObjects.Count;
                //Brute Force
                for (int i = 0; i < len; i++)//O(N^2)/2
                {
                    for (int j = i + 1; j < len; j++)
                    {
                        var obj1 = currentObjects[i];
                        var obj2 = currentObjects[j];
                        
                        if (CollisionHelper.Intersects(
                            obj1.Collider.CollisionShape,
                            obj2.Collider.CollisionShape))
                        {
                            lock (m_lock)
                            {
                                AddToBuffer(obj1.Id, obj2);
                                AddToBuffer(obj2.Id, obj1);
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
