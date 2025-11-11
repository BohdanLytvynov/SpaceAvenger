using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Attributes.Factories;
using WPFGameEngine.Enums;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Exponential
{
    [VisibleInEditor(FactoryName = nameof(EaseInExpo),
        DisplayName = "Ease In Expo f(t)= t = 0 -> 0; t > 0 -> 2^(10*(t-1))",
        GameObjectType = Enums.GEObjectType.Ease)]
    [BuildWithFactory<GEObjectType>(GameObjectType = GEObjectType.Ease)]
    public class EaseInExpo : EaseBase, IEase
    {
        public override double Ease(double t) => base.Ease(t) * (t == 0? 0 : System.Math.Pow(2, 10*(t - 1)));
    }
}
