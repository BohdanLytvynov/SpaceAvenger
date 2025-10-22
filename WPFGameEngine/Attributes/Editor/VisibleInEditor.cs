using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGameEngine.Attributes.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class VisibleInEditor : Attribute
    {
        public string Name { get; set; } = string.Empty;
    }
}
