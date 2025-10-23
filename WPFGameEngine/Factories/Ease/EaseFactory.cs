using WPFGameEngine.WPF.GE.Math.Ease.Base;
using WPFGameEngine.WPF.GE.Math.Ease.Polynomial;

namespace WPFGameEngine.Factories.Ease
{
    public class EaseFactory : IEaseFactory
    {
        public IEase Create(string name)
        {
            IEase ease = null;

            switch (name)
            {
                case nameof(LinearEase):
                    ease = new LinearEase();
                    break;
                case nameof(EaseInQuad):
                    ease = new EaseInQuad();
                    break;
                default:
                    throw new Exception("Unsupported Ease Type!");
                    break;
            }

            return ease;
        }
    }
}
