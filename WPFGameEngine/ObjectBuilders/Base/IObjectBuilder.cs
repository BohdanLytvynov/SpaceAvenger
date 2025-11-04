using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.ObjectBuilders.Base
{
    public interface IObjectBuilder
    {
        IFactoryWrapper FactoryWrapper { get; }

        MapableObject Build(string objName);

        TObject Build<TObject>() where TObject : MapableObject;
    }
}
