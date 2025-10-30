using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Component.Animators;

namespace WPFGameEngine.Factories.Components.Animators
{
    public class AnimatorFactory : FactoryBase, IAnimatorFactory
    {
        public AnimatorFactory()
        {
            ProductName = nameof(Animator);
        }

        public override IAnimator Create()
        {
            return new Animator();
        }
    }
}
