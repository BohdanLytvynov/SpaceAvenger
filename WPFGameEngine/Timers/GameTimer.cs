using System.Diagnostics;
using System.Windows.Media;

namespace WPFGameEngine.Timers
{
    public static class GameTimer
    {
        #region Delegates
        public static Action GameLoopFunction;
        #endregion

        #region Fields

        private static TimeSpan m_TotalTime;
        private static TimeSpan m_deltaTime;
        private static Stopwatch m_stopwatch;
        private static TimeSpan m_lastRenderTime;

        #endregion

        #region Properties
        /// <summary>
        /// Indicates if we need to perform Update Logic of the Game
        /// </summary>
        public static bool Started { get; set; }
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
            m_stopwatch = new Stopwatch();
        }
        #endregion

        public static void Init()
        {
            m_lastRenderTime = TimeSpan.Zero;

            CompositionTarget.Rendering += CompositionTarget_Rendering;
            m_stopwatch.Start();
        }

        private static void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            m_TotalTime = m_stopwatch.Elapsed;
            m_deltaTime = m_TotalTime - m_lastRenderTime;
            m_lastRenderTime = m_TotalTime;

            if(Started && GameLoopFunction is not null)
                GameLoopFunction();
        }

        public static void Stop()
        { 
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            m_stopwatch.Stop();
        }
    }
}
