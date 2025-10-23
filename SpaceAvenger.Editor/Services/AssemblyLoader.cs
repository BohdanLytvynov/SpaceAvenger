using SpaceAvenger.Editor.Services.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpaceAvenger.Editor.Services
{
    internal class AssemblyLoader : IAssemblyLoader
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
