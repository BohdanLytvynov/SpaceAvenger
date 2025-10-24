using System.Reflection;

namespace WPFGameEngine.Extensions
{
    public static class CustomAttributeDataExtensions
    {
        public static TValue GetValue<TValue>(this CustomAttributeData data, string name)
        {
            return (TValue)data.NamedArguments.Where(a => a.MemberName.Equals(name, StringComparison.OrdinalIgnoreCase))
                .Select(a => a.TypedValue.Value).FirstOrDefault();
        }
    }
}
