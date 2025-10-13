using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace WPFGameEngine.WPF.GE.Exceptions
{
    public class EmptyArgumentException : Exception
    {
        public EmptyArgumentException(string arg) : base("<" + arg + ">" + " was null or empty!")
        {

        }
    }
}
