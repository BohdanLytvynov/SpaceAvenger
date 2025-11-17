using System;
using System.Collections.Generic;
using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.ObjectBuilders.Base;
using WPFGameEngine.ObjectPools.Base;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;

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
            GetCollidedObjects = CollisionManager.GetObjects;
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
                    AddObject(obj);
                }
                else
                {
                    obj.StartUp();
                }
            }

            return obj;
        }

        public override void AddObject(IGameObject gameObject)
        {
            if (gameObject.IsCollidable)
                CollisionManager.AddObject(gameObject);
            base.AddObject(gameObject);
        }

        public override void AddObjects(IEnumerable<IGameObject> gameObjects)
        {
            foreach (IGameObject gameObject in gameObjects)
            {
                if (gameObject.IsCollidable)
                    CollisionManager.AddObject(gameObject);
            }
            base.AddObjects(gameObjects);
        }

        public override void RemoveObject(IGameObject gameObject)
        {
            CollisionManager.RemoveObject(gameObject);
            base.RemoveObject(gameObject);
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
