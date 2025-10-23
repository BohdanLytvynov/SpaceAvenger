using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpaceAvenger.Editor.Services.Base
{
    public interface IAssemblyLoader
    {
        public Dictionary<string, Assembly> LoadedAssemblies { get; }

        Assembly? LoadAssembly(string pathToFile);

        Assembly this [string name] { get; }
    }
}
