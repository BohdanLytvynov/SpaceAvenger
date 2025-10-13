using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGameEngine.Atributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DisplayInEditor : Attribute
    {
        public string NameInEditor { get; set; }
    }
}
