using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Dto.Base;

namespace WPFGameEngine.WPF.GE.Dto.Components
{
    public class AnimatorDto : DtoBase
    {
        public Dictionary<string, AnimationDto> NameAnimationMap { get; set; }

        public AnimatorDto()
        {
            NameAnimationMap = new Dictionary<string, AnimationDto>();
        }
    }
}
