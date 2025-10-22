using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGameEngine.Timers.Base
{
    public interface IGameTimer
    {
        public bool Started { get; }
        public TimeSpan deltaTime { get; }
        public TimeSpan totalTime { get; }
        void Start();
        void Stop();
        void ReInit();
        void UpdateTime();
    }
}
