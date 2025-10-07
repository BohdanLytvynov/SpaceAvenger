using System.Windows;
using System.Windows.Media;

namespace WPFGameEngine.GameViewControl
{
    public class GameViewHost : FrameworkElement
    {
        #region Fields
        private readonly VisualCollection m_visuals;
        #endregion

        #region Properties
        protected override int VisualChildrenCount
        {
            get { return m_visuals.Count; }
        }
        #endregion

        #region Ctor
        public GameViewHost()
        {
            m_visuals = new VisualCollection(this);
            this.CacheMode = new BitmapCache();
        }
        #endregion

        #region Methods
        public void AddVisual(DrawingVisual visual)
        {
            m_visuals.Add(visual);
        }

        public void RemoveVisual(DrawingVisual visual)
        {
            m_visuals.Remove(visual);
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= m_visuals.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return m_visuals[index];
        }

        public void ClearVisuals()
        {
            m_visuals.Clear();
        }
        #endregion
    }
}
