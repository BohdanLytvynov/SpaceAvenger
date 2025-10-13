using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGameEngine.WPF.GE.Exceptions
{
    public class ComponentNotFoundException : Exception
    {
        public ComponentNotFoundException(string componentName)
            : base("<" + componentName + "> was not found!")
        {
            
        }
    }
}
