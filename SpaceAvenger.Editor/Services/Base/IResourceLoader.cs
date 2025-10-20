using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpaceAvenger.Editor.Services.Base
{
    internal interface IResourceLoader
    {
        public ResourceDictionary ResourceDictionary { get; }

        IEnumerable<string> LoadAll();

        TResource? Load<TResource>(string key);
    }
}
