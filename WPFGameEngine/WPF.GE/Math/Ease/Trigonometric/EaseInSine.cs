using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Attributes.Factories;
using WPFGameEngine.Enums;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Trigonometric
{
    [VisibleInEditor(FactoryName = nameof(EaseInSine),
        DisplayName = "Ease In Sine f(t)= 1 - cos((Pi*t)/2)",
        GameObjectType = Enums.GEObjectType.Ease)]
    [BuildWithFactory<GEObjectType>(GameObjectType = GEObjectType.Ease)]
    public class EaseInSine : EaseBase, IEase
    {
        public override double Ease(double t) => base.Ease(t) * (1 - System.Math.Cos((System.Math.PI * t)/2));
    }
}
