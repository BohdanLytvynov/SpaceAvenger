using System.Windows.Media.Imaging;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.AnimationFrames;
using WPFGameEngine.WPF.GE.Component.Base.ImageComponents;
using WPFGameEngine.WPF.GE.Validation.Base;

namespace WPFGameEngine.WPF.GE.Component.Animations
{
    public interface IAnimation : IImageComponent<BitmapSource>, IValidatable
    {
        #region Properties
        public Dictionary<string, double> EaseConstants { get; }

        public bool Completed { get; }
        
        public double TotalTime { get; set; }

        public  string EaseFactoryName { get; set; }

        public string EaseType { get; set; }

        public int CurrentFrameIndex { get; }

        public bool IsRunning { get; }

        public bool Freeze { get; init; }

        public bool Reverse { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public int FrameCount { get; }

        public int FrameHeight { get; }

        public int FrameWidth { get; }

        public double AnimationSpeed { get; set; }

        public bool IsLooping { get; set; }
  
        public List<IAnimationFrame> AnimationFrames { get; }
        
        #endregion

        #region Functions

        void Update(IGameTimer gameTimer);

        void Start();

        void Stop();

        void Reset(bool reverse);

        BitmapSource GetCurrentFrame();

        #endregion
    }
}
