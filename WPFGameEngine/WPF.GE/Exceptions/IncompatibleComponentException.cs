using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WPFGameEngine.WPF.GE.Exceptions
{
    public class IncompatibleComponentException : Exception
    {
        public IncompatibleComponentException(string component, List<string> components)
            : base(BuildExceptionMessage(component, components))
        {
            
        }

        private static string BuildExceptionMessage(string component, List<string> components)
        { 
            StringBuilder sb = new StringBuilder();

            sb.Append($"The <{component}> is not compatible with existing components: ");

            foreach (var item in components)
            {
                sb.Append(item);
                sb.Append(", ");
            }
            sb.Append("!");

            return sb.ToString();
        }
    }
}
