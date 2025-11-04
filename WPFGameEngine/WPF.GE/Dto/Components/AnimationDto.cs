using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.WPF.GE.AnimationFrames;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Dto.Base;

namespace WPFGameEngine.WPF.GE.Dto.Components
{
    public class AnimationDto : ImageDto
    {
        public List<IAnimationFrame> AnimationFrames { get; set; }

        public double Duration { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public string EaseType { get; set; }
        public string EaseFactoryName { get; set; }
        public double AnimationSpeed { get; set; }
        public bool IsLooping { get; set; }
        public bool Reverse { get; set; }
        public bool Freeze { get; set; }
        public Dictionary<string, double> EaseConstants { get; set; }

        public AnimationDto() : base(nameof(Animation))
        {
            AnimationFrames = new List<IAnimationFrame>();
            EaseConstants = new Dictionary<string, double>();
        }

        public override IAnimation ToObject(IFactoryWrapper factoryWrapper)
        {
            var anim = new Animation(factoryWrapper.ResourceLoader);

            anim.TotalTime = Duration;
            anim.Rows = Rows;
            anim.Columns = Columns;
            anim.EaseType = EaseType;
            anim.EaseFactoryName = EaseFactoryName;
            anim.AnimationSpeed = AnimationSpeed;
            anim.IsLooping = IsLooping;
            anim.Reverse = Reverse;
            anim.Freeze = Freeze;
            anim.ResourceKey = ResourceKey;

            foreach (var c in EaseConstants)
            {
                anim.EaseConstants.Add(c.Key, c.Value);
            }

            foreach (var animationFrame in AnimationFrames)
            { 
                anim.AnimationFrames.Add(new AnimationFrame(animationFrame.Lifespan));
            }
            anim.Load(ResourceKey);
            return anim;
        }
    }
}
