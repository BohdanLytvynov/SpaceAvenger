using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Math.Ease.Base;
using WPFGameEngine.WPF.GE.Math.Ease.Polynomial;

namespace WPFGameEngine.Factories.Ease.Linear
{
    internal class LinearEaseFactory : FactoryBase, ILinearEaseFactory
    {
        public LinearEaseFactory()
        {
            ProductName = nameof(LinearEase);
        }

        public override LinearEase Create()
        {
            return new LinearEase();
        }
    }
}
