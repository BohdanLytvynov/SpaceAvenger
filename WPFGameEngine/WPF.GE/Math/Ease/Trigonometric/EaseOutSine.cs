using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Attributes.Factories;
using WPFGameEngine.Enums;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Trigonometric
{
    [VisibleInEditor(FactoryName = nameof(EaseOutSine),
        DisplayName = "Ease Out Sine f(t)= sin((Pi*t)/2)",
        GameObjectType = Enums.GEObjectType.Ease)]
    [BuildWithFactory<GEObjectType>(GameObjectType = GEObjectType.Ease)]
    public class EaseOutSine : EaseBase, IEase
    {
        public override double Ease(double t) => base.Ease(t) * System.Math.Sin((System.Math.PI * t)/2);
    }
}
