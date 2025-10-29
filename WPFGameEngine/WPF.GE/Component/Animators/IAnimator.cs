using System.Windows.Media.Imaging;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Base;

namespace WPFGameEngine.WPF.GE.Component.Animators
{
    public interface IAnimator : IGEComponent
    {
        public string Current_Animation_Name { get; }

        public IAnimation Current { get; }

        void AddAnimation(string name, IAnimation animation);

        void RemoveAnimation(string name);

        IAnimation? GetAnimation(string name);

        void SetAnimationForPlay(string animationName);

        void SetAnimationForPlay(string animationName, bool resetPrevAnim);

        void Start();

        void Stop();

        void Reset();

        void Update(IGameTimer gameTimer);

        BitmapSource GetCurrentFrame();

        public IAnimation? this[string key]
        {
            get;
            set;
        }

        bool Contains(string name);
    }
}
