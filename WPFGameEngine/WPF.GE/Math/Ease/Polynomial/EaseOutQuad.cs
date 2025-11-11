using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Attributes.Factories;
using WPFGameEngine.Enums;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Polynomial
{
    [VisibleInEditor(FactoryName = nameof(EaseOutQuad),
        DisplayName = "Ease Out Quad f(t)=t*(2-t)",
        GameObjectType = Enums.GEObjectType.Ease)]
    [BuildWithFactory<GEObjectType>(GameObjectType = GEObjectType.Ease)]
    public class EaseOutQuad : EaseBase, IEase
    {
        public override double Ease(double t) => base.Ease(t) * t * (2 - t);
    }
}
