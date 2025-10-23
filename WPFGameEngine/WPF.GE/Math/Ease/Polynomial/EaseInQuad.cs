using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Polynomial
{
    [VisibleInEditor(Name = nameof(EaseInQuad))]
    public class EaseInQuad : IEase
    {
        public double Ease(double t)
        {
            return t * t;
        }
    }
}
