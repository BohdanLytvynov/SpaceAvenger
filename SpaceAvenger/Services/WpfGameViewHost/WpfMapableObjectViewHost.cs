using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using WPFGameEngine.Enums;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.ObjectBuilders.Base;
using WPFGameEngine.ObjectPools.Base;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Math.Matrixes;

namespace SpaceAvenger.Services.WpfGameViewHost
{
    internal class WpfMapableObjectViewHost : WpfGameObjectViewHost, IMapableObjectViewHost, IColliderView
    {
        #region Fields
        public ICollisionManager CollisionManager { get; protected set; }
        #endregion

        public WpfMapableObjectViewHost(IGameTimer gameTimer, 
            IObjectBuilder objectBuilder,
            IObjectPoolManager objectPoolManager,
            ICollisionManager collisionManager) :
            base(gameTimer)
        {
            CollisionManager = collisionManager ?? throw new ArgumentNullException(nameof(collisionManager));
            CollisionManager.World = World;
            ObjectBuilder = objectBuilder ?? throw new ArgumentNullException(nameof(objectBuilder));
            ObjectPoolManager = objectPoolManager ?? throw new ArgumentNullException(nameof(objectPoolManager));
        }

        public IObjectPoolManager ObjectPoolManager { get; init; }
        public IObjectBuilder ObjectBuilder { get; init; }

        public TObject Instantinate<TObject>(bool useCache = true) 
            where TObject : СacheableObject
        {
            TObject obj = null;
            string name = typeof(TObject).Name;

            if (!useCache)
            {
                obj = ObjectBuilder.Build<TObject>();
                AddObject(obj);
            }
            else
            {
                obj = ObjectPoolManager.GetFromPool<TObject>();

                if (obj == null)
                {
                    obj = ObjectBuilder.Build<TObject>();
                    Debug.WriteLine("Build:" + typeof(TObject).Name);
                    AddObject(obj);
                }
                else
                {
                    Debug.WriteLine("Use From Pool:" + typeof(TObject).Name);
                    obj.OnGetFromPool();
                }
            }
            return obj;
        }

        protected override void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            m_gameTimer.UpdateTime();
            if (GameState == GameState.Running)
            {
                m_visualCollection.Clear();
                var world = World.OrderByDescending(x => x.ZIndex).ToList();
                using (DrawingContext dc = m_drawingSurface.RenderOpen())
                {
                    int count = world.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (world[i] != null)
                        {
                            int id = World[i].Id;
                            world[i].Update();
                            world[i].ProcessCollision(CollisionManager.GetCollisionInfo(id));
                            CollisionManager.RemoveFromBuffer(id);
                            world[i].Render(dc, Matrix3x3.Identity);
                        }
                    }
                }
                m_visualCollection.Add(m_drawingSurface);
                //Call External Update
                OnUpdate?.Invoke();
            }
        }

        public override void StartGame()
        {
            CollisionManager.Start();
            base.StartGame();
        }

        public override void Resume()
        {
            CollisionManager.Resume();
            base.Resume();
        }

        public override void Pause()
        {
            CollisionManager.Pause();
            base.Pause();
        }

        public override void Stop()
        {
            CollisionManager.Stop();
            base.Stop();
        }

        public override void ClearWorld()
        {
            CollisionManager.Clear();
            base.ClearWorld();
        }
    }
}
