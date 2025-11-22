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
using WPFGameEngine.WPF.GE.GameObjects.Collidable;
using WPFGameEngine.WPF.GE.GameObjects.Renderable;
using WPFGameEngine.WPF.GE.GameObjects.Updatable;
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

        public TObject Instantiate<TObject>(Action<IGameObject>? config = null, bool useCache = true) 
            where TObject : СacheableObject
        {
            return (TObject)Instantiate(typeof(TObject).Name, config, useCache);
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
                            if (world[i] is IUpdatable updatable)
                                updatable.Update();
                            if (world[i] is ICollidable collidable)
                                collidable.ProcessCollision(CollisionManager.GetCollisionInfo(id));
                                CollisionManager.RemoveFromBuffer(id);
                            if (world[i] is IRenderable renderable)
                                renderable.Render(dc, Matrix3x3.Identity);
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

        public СacheableObject Instantiate(string typeName, Action<IGameObject>? config = null, bool useCache = true)
        {
            СacheableObject? obj = null;
            
            if (!useCache)
            {
                obj = ObjectBuilder.Build(typeName) as СacheableObject;
                config?.Invoke(obj);
                AddObject(obj);
            }
            else
            {
                obj = ObjectPoolManager.GetFromPool(typeName);

                if (obj == null)
                {
                    obj = ObjectBuilder.Build(typeName) as СacheableObject;
                    config?.Invoke(obj);
                    Debug.WriteLine("Build:" + typeName);
                    AddObject(obj);
                }
                else
                {
                    Debug.WriteLine("Use From Pool:" + typeName);
                    obj.OnGetFromPool();
                }
            }
            return obj;
        }
    }
}
