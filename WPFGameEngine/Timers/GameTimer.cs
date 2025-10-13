using System.Diagnostics;
using System.Windows.Media;

namespace WPFGameEngine.Timers
{
    public static class GameTimer
    {
        #region Fields
        private static TimeSpan m_TotalTime;
        private static TimeSpan m_deltaTime;
        private static Stopwatch m_stopwatch;
        private static TimeSpan m_lastRenderTime;
        private static bool m_started;
        #endregion

        #region Properties
        /// <summary>
        /// Indicates if we need to perform Update Logic of the Game
        /// </summary>
        public static bool Started { get => m_started; }
        /// <summary>
        /// Time elapsed between frames
        /// </summary>
        public static TimeSpan deltaTime { get => m_deltaTime; }
        /// <summary>
        /// Time that passed after the game started
        /// </summary>
        public static TimeSpan totalTime { get => m_TotalTime; }
        #endregion

        #region Ctor
        static GameTimer()
        {
            ReInit();
        }
        #endregion

        public static void ReInit()
        {
            m_stopwatch = new Stopwatch();
            m_lastRenderTime = TimeSpan.Zero;
        }

        public static void Start()
        {
            if (!m_started)
            {
                m_stopwatch.Start();
                m_started = true;
            }
        }

        public static void UpdateTime()
        {
            if (m_started)
            {
                m_TotalTime = m_stopwatch.Elapsed;
                m_deltaTime = m_TotalTime - m_lastRenderTime;
                m_lastRenderTime = m_TotalTime;
            }
        }

        public static void Stop()
        {
            if (m_started)
            {
                m_stopwatch.Stop();
                m_started = false;
            }
        }
    }
}
