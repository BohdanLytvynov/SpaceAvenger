using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Polynomial
{
    [VisibleInEditor(FactoryName = nameof(LinearEase),
        DisplayName = "Linear Ease", 
        GameObjectType = Enums.GEObjectType.Ease)]
    
    public class LinearEase : IEase
    {
        public double B { get; set; } = 1;

        public double Ease(double t)
        {
            return B*t;
        }
    }
}
