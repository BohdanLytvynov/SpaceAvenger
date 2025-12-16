using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using WPFGameEngine.Enums;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.GameObjects.Renderable;
using WPFGameEngine.WPF.GE.GameObjects.Updatable;
using WPFGameEngine.WPF.GE.Math.Matrixes;

namespace SpaceAvenger.Services.WpfGameViewHost
{
    public class WpfGameObjectViewHost : FrameworkElement, IGameObjectViewHost
    {
        #region Delegates
        public Action OnUpdate;
        #endregion
         
        #region Fields
        protected readonly VisualCollection m_visualCollection;
        protected readonly DrawingVisual m_drawingSurface;
        private GameState m_gameState;
        protected readonly IGameTimer m_gameTimer;
        #endregion

        #region Properties
        public IGameTimer GameTimer { get => m_gameTimer; }
        public List<IGameObject> World { get; protected set; }
        public GameState GameState { get; protected set; }
        protected override int VisualChildrenCount => m_visualCollection.Count;
        #endregion

        #region Ctor
        public WpfGameObjectViewHost(
            IGameTimer gameTimer)
        {
            m_gameTimer = gameTimer ?? throw new ArgumentNullException(nameof(gameTimer));
            World = new List<IGameObject>();
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

        protected virtual void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            m_gameTimer.UpdateTime();
            if (m_gameState == GameState.Running)
            {
                m_visualCollection.Clear();
                World.Sort(new GameObject.ZIndexGameObjectComparer());
                using (DrawingContext dc = m_drawingSurface.RenderOpen())
                {
                    int count = World.Count;
                    for(int i = 0; i < count; i++)
                    {
                        if(World[i] != null)
                        {
                            if (World[i] is IUpdatable updatable)
                                updatable.Update();
                            if (World[i] is IRenderable renderable)
                                renderable.Render(dc, Matrix3x3.Identity);
                        }
                    }
                }
                m_visualCollection.Add(m_drawingSurface);
                //Call External Update
                OnUpdate?.Invoke();
            }
        }

        public void AddObject(IGameObject gameObject, Action<IGameObject> preStartUpConfig = null,
            Action<IGameObject> postStartUpConfig = null)
        {
            if (gameObject == null)
                throw new ArgumentNullException(nameof(gameObject));

            preStartUpConfig?.Invoke(gameObject);

            if(gameObject is IUpdatable updatable)
                updatable.StartUp(this, m_gameTimer);

            postStartUpConfig?.Invoke(gameObject);

            World.Add(gameObject);
        }

        public void RemoveObject(IGameObject gameObject)
        {
            if(gameObject == null)
                return;

            GameObject.RemoveObject(p => p.Id.Equals(gameObject.Id), World, true);
        }

        public virtual void StartGame()
        { 
            m_gameTimer.Start();
            m_gameState = GameState.Running;
        }

        public virtual void Resume()
        {
            m_gameState = GameState.Running;
        }

        public virtual void Pause()
        {
            m_gameState = GameState.Paused;
        }

        public virtual void Stop()
        {
            m_gameState = GameState.Stopped;
            m_gameTimer.Stop();
        }

        public virtual void ClearWorld()
        {
            World.Clear();
        }

        public IGameObject GetObject(Func<IGameObject, bool> predicate, bool recursiveSearch = false)
        {
            IGameObject res = null;

            foreach (var gameObject in World)
            {
                if (!recursiveSearch)
                {
                    if (predicate.Invoke(gameObject))
                    {
                        res = gameObject;
                        break;
                    }
                }
                else
                {
                    if (predicate.Invoke(gameObject))
                    {
                        res = gameObject;
                        break;
                    }
                    else
                    {
                        res = gameObject.FindChild(predicate, true);

                        if (res != null)
                            break;
                    }
                }
            }

            return res;
        }

        #endregion
    }
}
