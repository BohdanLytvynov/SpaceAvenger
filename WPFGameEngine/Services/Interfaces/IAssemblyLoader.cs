using System.Reflection;

namespace WPFGameEngine.Services.Interfaces
{
    public interface IAssemblyLoader
    {
        public Dictionary<string, Assembly> LoadedAssemblies { get; }

        Assembly? LoadAssembly(string pathToFile);

        Assembly this [string name] { get; }
    }
}
