using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGameEngine.Factories.Base
{
    public interface IAbstractFactory<TType>
    {
        TType Create(string name);
    }
}
