using WPFGameEngine.Factories.Base;

namespace WPFGameEngine.WPF.GE.Math.Ease.Base
{
    public interface IEase : IGameEngineEntity
    {
        public Dictionary<string,double> Constants { get; }

        double Ease(double t);

        double GetDelta(double y0, double y1);
    }
}
