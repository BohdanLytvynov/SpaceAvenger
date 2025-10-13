using SpaceAvenger.Editor.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpaceAvenger.Editor.Services
{
    internal class ResourceLoader : IResourceLoader
    {
        private string m_resourceDictionaryPath;

        public ResourceDictionary ResourceDictionary { get; set; }

        public ResourceLoader(string path)
        {
            m_resourceDictionaryPath = path;
        }

        public IEnumerable<string> LoadAll()
        {
            ResourceDictionary = new ResourceDictionary();
            ResourceDictionary.Source = new Uri(m_resourceDictionaryPath, UriKind.RelativeOrAbsolute);
            foreach (var k in ResourceDictionary.Keys)
            {
                yield return k.ToString();
            }
        }
    }
}
