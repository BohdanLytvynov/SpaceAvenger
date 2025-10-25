using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Constant
{
    [VisibleInEditor(DisplayName = "No Ease f(t)=C", FactoryName = nameof(NoEase),
        GameObjectType = Enums.GEObjectType.Ease)]
    public class NoEase : EaseBase, IEase
    {
        public override double Ease(double t)
        {
            return Constants["C"];
        }

        public override double GetDelta(double y0, double y1)
        {
            return Constants["C"];
        }

        public NoEase()
        {
            Constants.Add("C", 1);
        }
    }
}
