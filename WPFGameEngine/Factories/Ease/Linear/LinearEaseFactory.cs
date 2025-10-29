using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Math.Ease.Base;
using WPFGameEngine.WPF.GE.Math.Ease.Polynomial;

namespace WPFGameEngine.Factories.Ease.Linear
{
    internal class LinearEaseFactory : FactoryBase<LinearEase>, ILinearEaseFactory
    {
        public LinearEaseFactory() : base()
        {
            
        }

        public override LinearEase Create()
        {
            return new LinearEase();
        }
    }
}
