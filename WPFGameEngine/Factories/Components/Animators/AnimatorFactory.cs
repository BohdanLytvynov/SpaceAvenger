using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Component.Animators;

namespace WPFGameEngine.Factories.Components.Animators
{
    public class AnimatorFactory : FactoryBase<IAnimator>, IAnimatorFactory
    {
        public AnimatorFactory() : base()
        {
            
        }

        public override IAnimator Create()
        {
            return new Animator();
        }
    }
}
