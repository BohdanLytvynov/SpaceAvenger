using System.Reflection;

namespace WPFGameEngine.Extensions
{
    public static class TypeExtensions
    {
        public static CustomAttributeData? GetAttribute<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return type.CustomAttributes
                .Where(a => a.AttributeType.Name.Equals(typeof(TAttribute).Name,
                StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }
    }
}
