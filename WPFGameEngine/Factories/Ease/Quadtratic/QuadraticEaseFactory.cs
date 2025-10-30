using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Math.Ease.Polynomial;

namespace WPFGameEngine.Factories.Ease.Quadtratic
{
    internal class QuadraticEaseFactory : FactoryBase, IQuadraticEaseFactory
    {
        public QuadraticEaseFactory()
        {
            ProductName = nameof(EaseInQuad);
        }

        public override EaseInQuad Create()
        {
            return new EaseInQuad();
        }
    }
}
