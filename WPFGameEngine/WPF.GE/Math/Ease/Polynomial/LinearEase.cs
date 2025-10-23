using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Polynomial
{
    [VisibleInEditor(Name = nameof(LinearEase))]
    public class LinearEase : IEase
    {
        public double B { get; set; } = 1;

        public double Ease(double t)
        {
            return B*t;
        }
    }
}
