using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Polynomial
{
    [VisibleInEditor(FactoryName = nameof(EaseInQuad),
        DisplayName = "Ease In Quad", 
        GameObjectType = Enums.GEObjectType.Ease)]
    public class EaseInQuad : IEase
    {
        public double Ease(double t)
        {
            return t * t;
        }
    }
}
