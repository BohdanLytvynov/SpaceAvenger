using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using WPFGameEngine.Enums;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Math.Matrixes;

namespace SpaceAvenger.Services.WpfGameViewHost
{
    public class WpfGameObjectViewHost : FrameworkElement, IGameObjectViewHost
    {
        #region Delegates
        public Action OnUpdate;
        #endregion
         
        #region Fields
        private readonly VisualCollection m_visualCollection;
        private DrawingVisual m_drawingSurface;
        private List<IGameObject> m_world;

        private GameState m_gameState;
        private IGameTimer m_gameTimer;
        public ICollisionManager CollisionManager { get; protected set; }
        #endregion

        #region Properties
        public List<IGameObject> World { get => m_world; }
        public GameState GameState { get; }
        protected override int VisualChildrenCount => m_visualCollection.Count;
        #endregion

        #region Ctor
        public WpfGameObjectViewHost(
            IGameTimer gameTimer, 
            ICollisionManager collisionManager)
        {
            CollisionManager = collisionManager ?? throw new ArgumentNullException(nameof(collisionManager));
            m_gameTimer = gameTimer ?? throw new ArgumentNullException(nameof(gameTimer));
            m_world = new List<IGameObject>();
            m_drawingSurface = new DrawingVisual();
            m_visualCollection = new VisualCollection(this);
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        #endregion

        #region Mehtods
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= m_visualCollection.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return m_visualCollection[index];
        }

        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            m_gameTimer.UpdateTime();
            if (m_gameState == GameState.Running)
            {
                m_visualCollection.Clear();
                m_world.Sort(new GameObject.ZIndexGameObjectComparer());
                using (DrawingContext dc = m_drawingSurface.RenderOpen())
                {
                    int count = World.Count;
                    for(int i = 0; i < count; i++)
                    {
                        if(World[i] != null)
                        {
                            int id = World[i].Id;
                            World[i].Update(this, m_gameTimer);
                            World[i].ProcessCollision(CollisionManager.GetObjects(id));
                            World[i].Render(dc, Matrix3x3.Identity);
                        }
                    }
                }
                m_visualCollection.Add(m_drawingSurface);
                //Call External Update
                OnUpdate?.Invoke();
            }
        }

        public void AddObject(IGameObject gameObject)
        {
            if (gameObject == null)
                throw new ArgumentNullException(nameof(gameObject));

            gameObject.StartUp();
            m_world.Add(gameObject);

            if (gameObject.IsCollidable)
                CollisionManager.AddObject(gameObject);
        }

        public void AddObjects(IEnumerable<IGameObject> gameObjects)
        {
            if (gameObjects == null)
                return;

            if (gameObjects.Count() == 0)
                return;

            m_world.AddRange(gameObjects);

            foreach (IGameObject gameObject in gameObjects)
            {
                if (gameObject.IsCollidable)
                    CollisionManager.AddObject(gameObject);
            }
        }

        public void RemoveObject(IGameObject gameObject)
        {
            if(gameObject == null)
                return;

            GameObject.RemoveObject(p => p.ObjectName.Equals(gameObject.ObjectName), World, true);
            CollisionManager.RemoveObject(gameObject);
        }

        public void StartGame()
        { 
            m_gameTimer.Start();
            CollisionManager.Start();
            m_gameState = GameState.Running;
        }

        public void Resume()
        {
            CollisionManager.Resume();
            m_gameState = GameState.Running;
        }

        public void Pause()
        { 
            CollisionManager.Pause();
            m_gameState = GameState.Paused;
        }

        public void Stop()
        { 
            CollisionManager.Stop();
            m_gameState = GameState.Stopped;
            m_gameTimer.Stop();
        }

        public void ClearWorld()
        { 
            m_world.Clear();
            CollisionManager.Clear();
        }

        #endregion
    }
}
