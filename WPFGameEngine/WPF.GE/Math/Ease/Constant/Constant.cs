using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Constant
{
    [VisibleInEditor(DisplayName = "Constant Ease", FactoryName = nameof(ConstantEase),
        GameObjectType = Enums.GEObjectType.Ease)]
    public class ConstantEase : IEase
    {
        public double C { get; set; } = 1;
        public double Ease(double t)
        {
            return C;
        }
    }
}
