using System.Reflection;
using System.Resources;

namespace Domain.Services.StringResourceLoaders
{
    public class StringResourceLoader : IStringResourceLoader
    {
        private ResourceManager m_resourceManager;

        public StringResourceLoader(string pathToResx) : this(pathToResx, null)
        {
            
        }

        public StringResourceLoader(string pathToResx, Assembly assembly)
        {
            if (assembly == null)
                m_resourceManager = new ResourceManager(pathToResx,
                    Assembly.GetExecutingAssembly());
            else
                m_resourceManager = new ResourceManager(pathToResx, assembly);
        }

        public string GetString(string resourceName)
        {
            return m_resourceManager.GetString(resourceName);
        }
    }
}
