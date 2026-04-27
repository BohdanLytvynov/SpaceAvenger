using System.Runtime.CompilerServices;

namespace ViewModelBaseLibDotNetCore.Helpers
{
    public static class ExceptionHelper
    {
        public static T ThrowIfNull<T>(T obj, [CallerMemberName] string name = "")
            where T : class
        { 
            if(obj == null)
                throw new ArgumentNullException(name);
            else
                return obj;
        }
    }
}
