using System.Numerics;
using WPFGameEngine.CollisionDetection.Base;
using WPFGameEngine.CollisionDetection.CollisionMatrixes;
using WPFGameEngine.WPF.GE.Component.Collider;
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

        public CollisionData(ICollidable gameObject) : this(gameObject, Vector2.Zero, 0f)
        {
            
        }
    }

    public class CollisionManager : ThreadSafeCollisionManager<CollisionData, ICollidable>, ICollisionManager
    {
        #region Ctor
        public CollisionManager() : base()
        {
        }

        #endregion

        #region Methods
        /// <summary>
        /// Main Collision checker
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected override async Task CheckCollisions(CancellationToken token)
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

                m_currentCollidableObjects.Clear();//Clear before filtration
                //Good approach is to use froeach iterator instead of LINQs. 
                //Cause we should avoid lots of Allocations, and GC procedures
                //Filter all the objects, that can collide
                foreach (var obj in m_worldSnapshot)
                {
                    if (obj is ICollidable collidable &&
                        obj.Enabled && collidable.IsVisible &&
                        collidable.IsCollidable &&
                        collidable.ColliderComponent.CollisionEnabled)
                    {
                        m_currentCollidableObjects.Add(collidable);
                    }
                }

                int len = m_currentCollidableObjects.Count;//Length of the collection with objects, that can collide, we reduce calls to the Count property
                //Brute Force
                for (int i = 0; i < len; i++)//O(N^2)/2
                {
                    for (int j = i + 1; j < len; j++)
                    {
                        var obj1 = m_currentCollidableObjects[i];
                        var obj2 = m_currentCollidableObjects[j];

                        if (obj1.Id >= obj2.Id)//Avoiding of double check A - B and B - A
                            continue;
                        //Check if that objects should Collide
                        if (!CollisionMatrix.CanCollide(obj1.CollisionLayer, obj2.CollisionLayer))
                            continue;

                        //Collision Checking
                        var colInfo = CollisionHelper.Intersects(
                            (obj1.ColliderComponent as ICollider)!.CollisionShape,
                            (obj2.ColliderComponent as ICollider)!.CollisionShape);

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
        #endregion
    }
}