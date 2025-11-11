using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Attributes.Factories;
using WPFGameEngine.Enums;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Exponential
{
    [VisibleInEditor(FactoryName = nameof(EaseOutExpo),
        DisplayName = "Ease Out Expo f(t)= t = 1 -> 1; t < 1 -> 1-2^(-10*t)",
        GameObjectType = Enums.GEObjectType.Ease)]
    [BuildWithFactory<GEObjectType>(GameObjectType = GEObjectType.Ease)]
    public class EaseOutExpo : EaseBase, IEase
    {
        public override double Ease(double t) => base.Ease(t) * (t == 1 ? 1 : 1 - System.Math.Pow(2, (-10*t)));
    }
}
