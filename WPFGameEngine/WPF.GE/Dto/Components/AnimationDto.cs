using WPFGameEngine.WPF.GE.AnimationFrames;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Dto.Base;

namespace WPFGameEngine.WPF.GE.Dto.Components
{
    public class AnimationDto : ImageDto
    {
        public List<IAnimationFrame> AnimationFrames { get; set; }

        public AnimationDto() : base(nameof(Animation))
        {
            AnimationFrames = new List<IAnimationFrame>();
        }
    }
}
