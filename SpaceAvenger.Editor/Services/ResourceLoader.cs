using System.Windows;
using WPFGameEngine.Services.Interfaces;

namespace SpaceAvenger.Editor.Services
{
    internal class ResourceLoader : IResourceLoader
    {
        private string m_resourceDictionaryPath;
        private ResourceDictionary m_resourceDictionary;

        public ResourceDictionary ResourceDictionary 
        {
            get
            {
                if (m_resourceDictionary == null)
                {
                    m_resourceDictionary = new ResourceDictionary();
                    m_resourceDictionary.Source = new Uri(m_resourceDictionaryPath, UriKind.RelativeOrAbsolute);
                }

                return m_resourceDictionary;
            }
        }

        public ResourceLoader(string path)
        {
            m_resourceDictionaryPath = path;
        }

        public IEnumerable<string> GetAllKeys()
        {
            foreach (var k in ResourceDictionary.Keys)
            {
                yield return k.ToString();
            }
        }

        public TResource? Load<TResource>(string key)
        {
            if (ResourceDictionary.Contains(key))
            { 
                return (TResource)m_resourceDictionary[key];
            }

            return default;
        }
    }
}
