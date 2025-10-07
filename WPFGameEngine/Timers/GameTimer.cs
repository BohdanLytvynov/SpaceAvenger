using System.Diagnostics;
using System.Windows.Media;

namespace WPFGameEngine.Timers
{
    public static class GameTimer
    {
        public static Action UpdateFunction;

        public static TimeSpan deltaTime;

        private static Stopwatch m_stopwatch;

        private static TimeSpan m_lastRenderTime;

        public static bool Started { get; set; }

        public static void Init()
        {
            m_stopwatch = new Stopwatch();

            m_lastRenderTime = TimeSpan.Zero;

            CompositionTarget.Rendering += CompositionTarget_Rendering;
            m_stopwatch.Start();
        }
        
        //Usage TotalSeconds
        private static void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            TimeSpan totalTime = m_stopwatch.Elapsed;
            deltaTime = totalTime - m_lastRenderTime;
            m_lastRenderTime = totalTime;

            if(Started && UpdateFunction is not null)
                UpdateFunction();
        }

        public static void Stop()
        { 
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            m_stopwatch.Stop();
        }
    }
}
