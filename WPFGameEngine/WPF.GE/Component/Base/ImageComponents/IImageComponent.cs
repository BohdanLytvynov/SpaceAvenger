using System.Windows.Media;
using WPFGameEngine.Services.Interfaces;

namespace WPFGameEngine.WPF.GE.Component.Base.ImageComponents
{
    public interface IImageComponent<TImage> : IGEComponent
        where TImage : ImageSource
    {
        public IResourceLoader ResourceLoader { get; }

        public string ResourceKey { get; set; }

        public TImage Texture { get; }

        void Load(string resourceKey);

        void TryLoad(string resourceName);
    }
}
