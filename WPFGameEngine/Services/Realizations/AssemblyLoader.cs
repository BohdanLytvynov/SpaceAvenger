using System.IO;
using System.Reflection;
using WPFGameEngine.Services.Interfaces;

namespace WPFGameEngine.Services.Realizations
{
    public class AssemblyLoader : IAssemblyLoader
    {
        #region Propperties

        public Dictionary<string, Assembly> LoadedAssemblies { get; }

        public Assembly this[string name]
        { 
            get => LoadedAssemblies[name];
        }

        #endregion

        #region Ctor
        public AssemblyLoader()
        {
            LoadedAssemblies = new Dictionary<string, Assembly>();
        }
        
        public Assembly? LoadAssembly(string pathToFile)
        {
            var assemblyName = Path.GetFileName(pathToFile).Split(".").FirstOrDefault();

            if (string.IsNullOrEmpty(assemblyName))
                return null;

            if (!LoadedAssemblies.ContainsKey(assemblyName))
            {
                try
                {
                    var assembly = Assembly.LoadFile(pathToFile);
                    if (assembly == null)
                        return null;

                    LoadedAssemblies.Add(assemblyName, assembly);
                    return assembly;
                }
                catch (Exception ex)
                {

                }
            }
            else
            { 
                return LoadedAssemblies[assemblyName];
            }

            return null;
        }


        #endregion
    }
}
