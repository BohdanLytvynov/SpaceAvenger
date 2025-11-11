using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Attributes.Factories;
using WPFGameEngine.Enums;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Trigonometric
{
    [VisibleInEditor(FactoryName = nameof(EaseInCirc),
        DisplayName = "Ease In Circ f(t)= 1 - (1 - t^2)^1/2",
        GameObjectType = Enums.GEObjectType.Ease)]
    [BuildWithFactory<GEObjectType>(GameObjectType = GEObjectType.Ease)]
    public class EaseInCirc : EaseBase, IEase
    {
        public override double Ease(double t) => base.Ease(t) * (1 - System.Math.Sqrt(1 - t * t));
    }
}
