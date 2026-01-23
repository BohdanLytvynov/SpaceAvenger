using System.Numerics;
using WPFGameEngine.CollisionDetection.Base;
using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using WPFGameEngine.CollisionDetection.CollisionMatrixes;
using WPFGameEngine.WPF.GE.GameObjects.Collidable;
using WPFGameEngine.WPF.GE.GameObjects.Raycastable;
using WPFGameEngine.WPF.GE.Settings;
using static WPFGameEngine.WPF.GE.Helpers.RayCastHelper;

namespace WPFGameEngine.CollisionDetection.RaycastManager
{
    public struct RaycastData
    {
        public ICollidable Object { get; set; }
        /// <summary>
        /// Is Collision happened?
        /// </summary>
        public bool IsHit { get; set; }
        /// <summary>
        /// Point of Collision
        /// </summary>
        public Vector2 Point { get; set; }
        /// <summary>
        /// Surface Normal
        /// </summary>
        public Vector2 Normal { get; set; }
        /// <summary>
        /// Parameter t [0;1]
        /// </summary>
        public float T { get; set; }

        /// <summary>
        /// Main ctor
        /// </summary>
        /// <param name="isHit"></param>
        /// <param name="point"></param>
        /// <param name="normal"></param>
        /// <param name="t"></param>
        public RaycastData(bool isHit, Vector2 point, Vector2 normal, float t,
            ICollidable collidable)
        {
            Object = collidable;
            IsHit = isHit;
            Point = point;
            Normal = normal;
            T = t;
        }
    }
    public class RaycastManager : ThreadSafeCollisionManager<RaycastData, IRaycastable>, 
        IRaycastManager
    {
        List<ICollidable> m_collidableFilteredObjects;
        private object m_posLock;
        private Vector2 m_prev;
        private Vector2 m_curr;

        public RaycastManager() : base()
        {
            m_collidableFilteredObjects = new List<ICollidable>(128);
            m_posLock = new object();
        }

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
                //Filter all the objects, that can raycast
                foreach (var obj in m_worldSnapshot)
                {
                    if (obj is IRaycastable raycastable &&
                        obj.Enabled && raycastable.IsVisible &&
                        raycastable.IsRaycastable &&
                        raycastable.RaycastComponent.CollisionEnabled)
                    {
                        m_currentCollidableObjects.Add(raycastable);
                    }

                    if (obj is ICollidable collidable &&
                        obj.Enabled && collidable.IsVisible &&
                        collidable.IsCollidable &&
                        collidable.Collider.CollisionEnabled)
                    { 
                        m_collidableFilteredObjects.Add(collidable);
                    }
                }

                //Length of the collection with objects, that can collide, we reduce calls to the Count property
                int lenRayCast = m_currentCollidableObjects.Count;
                int lenRayCol = m_collidableFilteredObjects.Count;

                for (int i = 0; i < lenRayCast; i++)
                {
                    for (int j = 0; j < lenRayCol; j++)
                    {
                        var obj1 = m_currentCollidableObjects[i];
                        var obj2 = m_collidableFilteredObjects[j];

                        if (obj1.Id >= obj2.Id)//Avoiding of double check A - B and B - A
                            continue;
                        //Check if that objects should Collide
                        if (!CollisionMatrix.CanCollide(obj1.CollisionLayer, obj2.CollisionLayer))
                            continue;
                        //Prev and Current Position data shouldn't change during calculation
                        lock (m_posLock)
                        {
                            m_prev = obj1.RaycastComponent.PreviousPosition;
                            m_curr = obj1.RaycastComponent.CurrentPosition;
                        }

                        //Collision Checking
                        var hitData = RaycastHelper.RayIntersectsShape(
                            m_prev,
                            m_curr,
                            obj2.Collider.CollisionShape
                            );

                        if (hitData.IsHit)
                        {
                            AddToBackBuffer(obj1.Id, new RaycastData(
                                hitData.IsHit, 
                                hitData.Point, 
                                hitData.Normal, 
                                hitData.T, obj2));
                        }
                    }
                }

                SwapBuffers();
                PrepareBackBuffer();

                try
                {
                    //We use this to have some delay, 16 ms by default,
                    //again it can handle task cancellation scenario
                    await Task.Delay(CollisionSettings.RaycastCheckDelay_MS, token);
                }
                catch (TaskCanceledException)
                {
                    break;//Case of task cancellation
                }
            }
        }
    }
}
