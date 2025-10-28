using System.Windows.Media;
using WPFGameEngine.Services.Interfaces;

namespace WPFGameEngine.WPF.GE.Component.Base.ImageComponents
{
    public interface IImageComponent<TImage> : IGEComponent
        where TImage : ImageSource
    {
        public IResourceLoader ResourceLoader { get; }

        public string ResourceKey { get; }

        public TImage Texture { get; }

        void Load(string resourceKey);
    }
}
