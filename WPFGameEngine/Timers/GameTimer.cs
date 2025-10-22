using System.Diagnostics;
using System.Windows.Media;
using WPFGameEngine.Timers.Base;

namespace WPFGameEngine.Timers
{
    public class GameTimer : IGameTimer
    {
        #region Fields
        private TimeSpan m_TotalTime;
        private TimeSpan m_deltaTime;
        private Stopwatch m_stopwatch;
        private TimeSpan m_lastRenderTime;
        private bool m_started;
        #endregion

        #region Properties
        /// <summary>
        /// Indicates if we need to perform Update Logic of the Game
        /// </summary>
        public bool Started { get => m_started; }
        /// <summary>
        /// Time elapsed between frames
        /// </summary>
        public TimeSpan deltaTime { get => m_deltaTime; }
        /// <summary>
        /// Time that passed after the game started
        /// </summary>
        public TimeSpan totalTime { get => m_TotalTime; }
        #endregion

        #region Ctor
        public GameTimer()
        {
            ReInit();
        }
        #endregion

        public void ReInit()
        {
            m_stopwatch = new Stopwatch();
            m_lastRenderTime = TimeSpan.Zero;
        }

        public void Start()
        {
            if (!m_started)
            {
                m_stopwatch.Start();
                m_started = true;
            }
        }

        public void UpdateTime()
        {
            if (m_started)
            {
                m_TotalTime = m_stopwatch.Elapsed;
                m_deltaTime = m_TotalTime - m_lastRenderTime;
                m_lastRenderTime = m_TotalTime;
            }
        }

        public void Stop()
        {
            if (m_started)
            {
                m_stopwatch.Stop();
                m_started = false;
            }
        }
    }
}
