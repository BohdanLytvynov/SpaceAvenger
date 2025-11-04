using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Dto.Base;

namespace WPFGameEngine.WPF.GE.Dto.Components
{
    public class AnimatorDto : ComponentDto
    {
        public Dictionary<string, AnimationDto> NameAnimationMap { get; set; }

        public AnimatorDto()
        {
            NameAnimationMap = new Dictionary<string, AnimationDto>();
        }

        public override IAnimator ToObject(IFactoryWrapper factoryWrapper)
        {
            Animator animator = new Animator();

            foreach (var name in NameAnimationMap)
            {
                animator.AddAnimation(name.Key, (Animation)name.Value.ToObject(factoryWrapper));
            }

            return animator;
        }
    }
}
