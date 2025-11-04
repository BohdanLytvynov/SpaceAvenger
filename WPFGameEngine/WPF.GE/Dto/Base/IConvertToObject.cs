using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGameEngine.Factories.Base;
using WPFGameEngine.FactoryWrapper.Base;

namespace WPFGameEngine.WPF.GE.Dto.Base
{
    public interface IConvertToObject<TObject>
        where TObject : IGameEngineEntity
    {
        TObject ToObject(IFactoryWrapper factoryWrapper);
    }
}
