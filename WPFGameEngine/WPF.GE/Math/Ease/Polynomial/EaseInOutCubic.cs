using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Attributes.Factories;
using WPFGameEngine.Enums;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Polynomial
{
    [VisibleInEditor(FactoryName = nameof(EaseInOutCubic),
        DisplayName = "Ease In Out Cubic f(t)= t<0.5 -> 4*t^3; t>0.5 -> (1-(-2t+2)^3)/2",
        GameObjectType = Enums.GEObjectType.Ease)]
    [BuildWithFactory<GEObjectType>(GameObjectType = GEObjectType.Ease)]
    public class EaseInOutCubic : EaseBase, IEase
    {
        public override double Ease(double t) => base.Ease(t) * (t < 0.5 ? 4 * t * t * t : (1 - System.Math.Pow((-2 * t + 2), 3)) / 2);
    }
}
