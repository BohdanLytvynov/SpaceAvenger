using System;
using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.ObjectBuilders.Base;
using WPFGameEngine.ObjectPools.Base;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Services.WpfGameViewHost
{
    internal class WpfMapableObjectViewHost : WpfGameObjectViewHost, IMapableObjectViewHost
    {
        public WpfMapableObjectViewHost(IGameTimer gameTimer, IObjectBuilder objectBuilder, IObjectPoolManager objectPoolManager,
            ICollisionManager collisionManager) :
            base(gameTimer, collisionManager)
        {
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
    }
}
