using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Polynomial
{
    [VisibleInEditor(FactoryName = nameof(LinearEase),
        DisplayName = "Linear Ease f(t)=b*t", 
        GameObjectType = Enums.GEObjectType.Ease)]    
    public class LinearEase : EaseBase, IEase
    {
        public override double Ease(double t)
        {
            return t*Constants["B"];
        }

        public LinearEase()
        {
            Constants.Add("B", 1);
        }
    }
}
