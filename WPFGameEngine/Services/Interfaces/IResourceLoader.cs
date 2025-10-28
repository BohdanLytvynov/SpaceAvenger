using System.Windows;

namespace WPFGameEngine.Services.Interfaces
{
    public interface IResourceLoader
    {
        public ResourceDictionary ResourceDictionary { get; }

        IEnumerable<string> GetAllKeys();

        TResource? Load<TResource>(string key);
    }
}
