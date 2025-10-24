using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGameEngine.Enums;

namespace WPFGameEngine.Attributes.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class VisibleInEditor : Attribute
    {
        public string FactoryName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public GEObjectType GameObjectType { get; set; }
    }
}
