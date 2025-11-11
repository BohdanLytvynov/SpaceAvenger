using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Attributes.Factories;
using WPFGameEngine.Enums;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Polynomial
{
    [VisibleInEditor(FactoryName = nameof(EaseInOutQuad),
        DisplayName = "Ease In Out Quad f(t)= t<0.5 -> 2*t^2; t>0.5 -> (1-(-2t+2)^2)/2",
        GameObjectType = Enums.GEObjectType.Ease)]
    [BuildWithFactory<GEObjectType>(GameObjectType = GEObjectType.Ease)]
    public class EaseInOutQuad : EaseBase, IEase
    {
        public override double Ease(double t) => base.Ease(t) * (t < 0.5 ? 2 * t * t : (1 - System.Math.Pow((-2 * t + 2), 2)) / 2);
    }
}
