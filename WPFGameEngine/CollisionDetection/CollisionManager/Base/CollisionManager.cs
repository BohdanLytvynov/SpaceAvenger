using System.Collections.Concurrent;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Helpers;

namespace WPFGameEngine.CollisionDetection.CollisionManager.Base
{
    public class CollisionManager : ICollisionManager
    {
        #region Fields
        private object _lock;
        private List<IGameObject> m_CollidableObjects;
        private bool m_running;
        private CancellationTokenSource m_cancellationTokenSource;
        private Task m_check;

        private ConcurrentDictionary<int, List<IGameObject>> m_CollisionBuffer;
        #endregion

        #region Ctor
        public CollisionManager()
        {
            _lock = new object();
            m_CollisionBuffer = new ConcurrentDictionary<int, List<IGameObject>>();
            m_CollidableObjects = new List<IGameObject>();
            m_cancellationTokenSource = new CancellationTokenSource();
            m_running = false;
        }

        #endregion

        #region Methods
        public void AddObject(IGameObject obj)
        {
            lock (_lock)
            {
                m_CollidableObjects.Add(obj);
            }
        }

        public void RemoveObject(IGameObject obj)
        {
            lock (_lock)
            {
                m_CollidableObjects.Remove(obj);
            }

            List<IGameObject> _obj = null;
        }

        public void Start()
        {
            if (!m_running)
            {
                m_check.Start();
                m_running = true;
            }
            
        }

        public void Pause()
        { 
            if(m_running)
                m_running=false;
        }

        public void Stop()
        {
            if (m_running)
            { 
                m_cancellationTokenSource.Cancel();
                m_running = false;
            }    
        }

        public void CheckCollisions()
        {
            m_check = new Task(() =>
            {
                while (true)
                {
                    if (!m_running)
                        continue;
                    if (m_cancellationTokenSource.IsCancellationRequested)
                        break;
                    int len = m_CollidableObjects.Count;
                    //Brute Force
                    for (int i = 0; i < len; i++)//O(N^2)
                    {
                        for (int j = 0; j < len; j++)
                        { 
                            var obj1 = m_CollidableObjects[i];
                            var obj2 = m_CollidableObjects[j];
                            if (CollisionHelper.Intersects(
                                obj1.Collider.CollisionShape, 
                                obj2.Collider.CollisionShape))
                            { 
                                
                            }
                        }
                    }
                }
            });
        }
        #endregion
    }
}
