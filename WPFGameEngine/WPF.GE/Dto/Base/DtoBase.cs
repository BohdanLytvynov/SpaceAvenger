using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGameEngine.WPF.GE.Dto.Base
{
    public abstract class DtoBase
    {
        public string TypeName { get; set; }
        
        public DtoBase(string typeName) 
        {
            TypeName = typeName;
        }        
    }
}
