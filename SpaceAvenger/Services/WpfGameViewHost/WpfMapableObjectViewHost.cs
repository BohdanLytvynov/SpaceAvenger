using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using WPFGameEngine.Enums;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.ObjectBuilders.Base;
using WPFGameEngine.ObjectInstantiators;
using WPFGameEngine.ObjectPools.Base;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.GameObjects.Collidable;
using WPFGameEngine.WPF.GE.GameObjects.Renderable;
using WPFGameEngine.WPF.GE.GameObjects.Updatable;
using WPFGameEngine.WPF.GE.Math.Matrixes;

namespace SpaceAvenger.Services.WpfGameViewHost
{
    public class WpfMapableObjectViewHost : WpfGameObjectViewHost, IMapableObjectViewHost, IColliderView
    {
        #region Fields
        public ICollisionManager CollisionManager { get; init; }
        public IObjectInstantiator ObjectInstantiator { get; init; }
        #endregion

        public WpfMapableObjectViewHost(IGameTimer gameTimer, 
            IObjectInstantiator objectInstantiator,
            ICollisionManager collisionManager) :
            base(gameTimer)
        {
            CollisionManager = collisionManager ?? throw new ArgumentNullException(nameof(collisionManager));
            CollisionManager.World = World;
            ObjectInstantiator = objectInstantiator ?? throw new ArgumentNullException(nameof(objectInstantiator));
        }

        public TObject Instantiate<TObject>(Action<IGameObject>? preStartUpConfig = null,
            Action<IGameObject> postStartUpConfig = null, bool useCache = true) 
            where TObject : СacheableObject
        {
            return (TObject)Instantiate(typeof(TObject).Name, preStartUpConfig, postStartUpConfig, useCache);
        }

        protected override void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            m_gameTimer.UpdateTime();
            if (GameState == GameState.Running)
            {
                ObjectInstantiator.Update(m_gameTimer.totalTime.TotalMilliseconds);
                m_visualCollection.Clear();
                var world = World.OrderByDescending(x => x.ZIndex).ToList();
                using (DrawingContext dc = m_drawingSurface.RenderOpen())
                {
                    int count = world.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (world[i] != null)
                        {
                            int id = world[i].Id;
                            if (world[i] is IUpdatable updatable)
                                updatable.Update();
                            if (world[i] is ICollidable collidable)
                                collidable.ProcessCollision(CollisionManager.GetCollisionInfo(id));
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
            ObjectInstantiator.Clear();
        }

        public СacheableObject Instantiate(string typeName, 
            Action<IGameObject>? preStartUpConfig = null,
            Action<IGameObject>? postStartUpConfig = null, 
            bool useCache = true)
        {
            bool poolUsed;
            СacheableObject? obj = ObjectInstantiator.Instantiate(typeName, out poolUsed, useCache) as СacheableObject;
            if (!poolUsed)
            {
                AddObject(obj, preStartUpConfig, postStartUpConfig);
            }
            return obj;
        }
    }
}
