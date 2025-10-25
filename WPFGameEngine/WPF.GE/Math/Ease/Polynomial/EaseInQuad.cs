using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Polynomial
{
    [VisibleInEditor(FactoryName = nameof(EaseInQuad),
        DisplayName = "Ease In Quad f(t)=t^2", 
        GameObjectType = Enums.GEObjectType.Ease)]
    public class EaseInQuad : EaseBase, IEase
    {
        public override double Ease(double t)
        {
            return t * t;
        }
    }
}
