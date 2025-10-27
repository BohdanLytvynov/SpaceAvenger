using System.Windows.Media.Imaging;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Base;

namespace WPFGameEngine.WPF.GE.Component.Animators
{
    [VisibleInEditor(FactoryName = nameof(Animator),
        DisplayName = "Animator",
        GameObjectType = Enums.GEObjectType.Component)]
    public class Animator : ComponentBase, IAnimator
    {
        private Dictionary<string, IAnimation> m_animations;

        private IAnimation m_current;

        private string m_current_animation_name;

        public string Current_Animation_Name { get => m_current_animation_name; }

        public IAnimation Current => m_current;

        public Animator(Dictionary<string, IAnimation> animations = null) : base(nameof(Animator))
        {
            if (animations is null)
                m_animations = new Dictionary<string, IAnimation>();
            else
                m_animations = animations;
        }

        public Animator() : base(nameof(Animator))
        {
            m_animations = new Dictionary<string, IAnimation>();
        }

        public void AddAnimation(string name, IAnimation animation)
        {
            m_animations.Add(name, animation);
        }

        public void RemoveAnimation(string name)
        {
            m_animations.Remove(name);
        }

        public IAnimation? GetAnimation(string name)
        {
            return m_animations[name];
        }

        public void SetAnimationForPlay(string animationName)
        {
            m_current = m_animations[animationName];
            m_current_animation_name = animationName;
        }

        public void Start()
        {
            m_current.Start();
        }

        public void Stop()
        {
            m_current.Stop();
        }

        public void Reset()
        {
            m_current.Reset(m_current.Reverse);
        }

        public void Update(IGameTimer gameTimer)
        {
            m_current.Update(gameTimer);
        }

        public BitmapSource GetCurrentFrame()
        { 
            return m_current.GetCurrentFrame();
        }

        public void SetAnimationForPlay(string animationName, bool resetPrevAnim)
        {
            if (resetPrevAnim)
            {
                m_current.Stop();
                m_current.Reset(m_current.Reverse);
                m_current = this[animationName];
                m_current.Start();
            }
            else
            {
                m_current.Stop();
                m_current = this[animationName];
                m_current.Start();
            }

            m_current_animation_name = animationName;
        }

        public bool Contains(string name)
        {
            return m_animations.ContainsKey(name);
        }

        public IAnimation? this[string key]
        {
            get => m_animations[key];
            set 
            {
                if(!Contains(key))
                    m_animations[key] = value; 
            }
        }
    }
}
