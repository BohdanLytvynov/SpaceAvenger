using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Attributes.Factories;
using WPFGameEngine.Enums;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Polynomial
{
    [VisibleInEditor(FactoryName = nameof(EaseInQuad),
        DisplayName = "Ease In Quad f(t)=t^2", 
        GameObjectType = Enums.GEObjectType.Ease)]
    [BuildWithFactory<GEObjectType>(GameObjectType = GEObjectType.Ease)]
    public class EaseInQuad : EaseBase, IEase
    {
        public override double Ease(double t) => base.Ease(t) * t * t;
    }
}
