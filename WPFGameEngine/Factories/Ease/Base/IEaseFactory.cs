using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Math.Ease.Base;

namespace WPFGameEngine.Factories.Ease.Base
{
    public interface IEaseFactory<EaseType> : IFactory
        where EaseType : IEase
    {
    }
}
