namespace WPFGameEngine.WPF.GE.Math.Ease.Base
{
    public abstract class EaseBase : IEase
    {
        public Dictionary<string, double> Constants { get; }

        public virtual double Ease(double t)
        {
            return Constants["A"] + (Constants["B"] - Constants["A"]);
        }
        
        protected EaseBase()
        {
            Constants = new Dictionary<string, double>();
            Constants.Add("A", 0);
            Constants.Add("B", 1);
        }
    }
}
