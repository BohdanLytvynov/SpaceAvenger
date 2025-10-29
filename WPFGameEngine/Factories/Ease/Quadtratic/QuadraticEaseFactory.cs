using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Math.Ease.Polynomial;

namespace WPFGameEngine.Factories.Ease.Quadtratic
{
    internal class QuadraticEaseFactory : FactoryBase<EaseInQuad>, IQuadraticEaseFactory
    {
        public QuadraticEaseFactory() : base()
        {
            
        }

        public override EaseInQuad Create()
        {
            return new EaseInQuad();
        }
    }
}
