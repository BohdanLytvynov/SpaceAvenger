using WPFGameEngine.WPF.GE.Component.Base;

namespace WPFGameEngine.Factories.Base
{
    public interface IAbstractFactory<TType>
    {
        TType Create(string name);
    }
}
