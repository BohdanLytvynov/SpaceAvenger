namespace WPFGameEngine.WPF.GE.Math.Ease.Base
{
    public abstract class EaseBase : IEase
    {
        public Dictionary<string, double> Constants { get; }

        public abstract double Ease(double t);

        public virtual double GetDelta(double y0, double y1)
        {
            return y1 - y0;
        }

        protected EaseBase()
        {
            Constants = new Dictionary<string, double>();
        }
    }
}
