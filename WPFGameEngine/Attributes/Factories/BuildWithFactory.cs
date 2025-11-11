using WPFGameEngine.Enums;

namespace WPFGameEngine.Attributes.Factories
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BuildWithFactory<ObjectGroup> : Attribute
        where ObjectGroup : struct, Enum
    {
        public ObjectGroup GameObjectType { get; set; }
    }
}
