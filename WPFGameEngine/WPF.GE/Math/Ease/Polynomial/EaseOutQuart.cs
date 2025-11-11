using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Attributes.Factories;
using WPFGameEngine.Enums;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Polynomial
{
    [VisibleInEditor(FactoryName = nameof(EaseOutQuart),
        DisplayName = "Ease Out Quart f(t)=1 - (1 - t)^4",
        GameObjectType = Enums.GEObjectType.Ease)]
    [BuildWithFactory<GEObjectType>(GameObjectType = GEObjectType.Ease)]
    public class EaseOutQuart : EaseBase, IEase
    {
        public override double Ease(double t) => base.Ease(t) * (1 - System.Math.Pow((1 - t), 4));
    }
}
