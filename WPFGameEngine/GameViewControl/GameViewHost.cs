using System.Windows;
using System.Windows.Media;
using WPFGameEngine.Enums;
using WPFGameEngine.Timers;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.GameViewControl
{
    public class GameViewHost : FrameworkElement
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
        #endregion

        #region Properties
        public List<IGameObject> World { get => m_world; }
        public GameState GameState { get; }
        protected override int VisualChildrenCount => m_visualCollection.Count;
        #endregion

        #region Ctor
        public GameViewHost(IGameTimer gameTimer)
        {
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
                    foreach (var obj in World)
                    {
                        if(obj != null)
                        {
                            obj.Update(World, m_gameTimer);
                            obj.Render(dc, Matrix.Identity);
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
                return;

            gameObject.StartUp();
            m_world.Add(gameObject);
        }

        public void AddObjects(IEnumerable<IGameObject> gameObjects)
        {
            if (gameObjects == null)
                return;

            if (gameObjects.Count() == 0)
                return;

            m_world.AddRange(gameObjects);
        }

        public void RemoveObject(IGameObject gameObject)
        {
            if(gameObject == null)
                return;

            GameObject.RemoveObject(p => p.ObjectName.Equals(gameObject.ObjectName), World, true);
        }

        public void StartGame()
        { 
            m_gameTimer.Start();
            m_gameState = GameState.Running;
        }

        public void Resume()
        {
            m_gameState = GameState.Running;
        }

        public void Pause()
        { 
            m_gameState = GameState.Paused;
        }

        public void Stop()
        { 
            m_gameState = GameState.Stopped;
            m_gameTimer.Stop();
        }

        public void ClearWorld()
        { 
            m_world.Clear();
        }
        #endregion
    }
}
