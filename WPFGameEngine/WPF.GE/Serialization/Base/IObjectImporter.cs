using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGameEngine.WPF.GE.Serialization.Base
{
    internal interface IObjectImporter<out T>
    {
        T ImportObject(string pathToFolder, string objName);
    }
}
