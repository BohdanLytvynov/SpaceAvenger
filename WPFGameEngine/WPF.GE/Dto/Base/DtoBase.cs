using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGameEngine.Factories.Base;

namespace WPFGameEngine.WPF.GE.Dto.Base
{
    public abstract class DtoBase : IGameEngineEntity
    {
        public string TypeName { get; set; }
        
        public DtoBase(string typeName) 
        {
            TypeName = typeName;
        }        
    }
}
