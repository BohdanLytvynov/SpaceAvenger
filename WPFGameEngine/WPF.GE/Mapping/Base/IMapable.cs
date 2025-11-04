using WPFGameEngine.FactoryWrapper.Base;

namespace WPFGameEngine.WPF.GE.Mapping.Base
{
    public interface IMapable<TSrc>
    {
        IFactoryWrapper FactoryWrapper { get; set; }

        void Map(TSrc src);
    }
}
