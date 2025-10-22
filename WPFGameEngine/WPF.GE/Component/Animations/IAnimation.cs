using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Base;

namespace WPFGameEngine.WPF.GE.Component.Animations
{
    public interface IAnimation : IGEComponent
    {
        #region Properties

        public bool Freeze { get; init; }

        public bool Reverse { get; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public int FrameCount { get; }

        public int FrameHeight { get; }

        public int FrameWidth { get; }

        public double AnimationSpeed { get; set; }

        public bool IsLooping { get; set; }

        public BitmapSource Texture { get; }

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
