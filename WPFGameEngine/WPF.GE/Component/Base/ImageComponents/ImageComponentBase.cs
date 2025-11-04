using System.Windows.Media;
using WPFGameEngine.Services.Interfaces;

namespace WPFGameEngine.WPF.GE.Component.Base.ImageComponents
{
    public abstract class ImageComponentBase<TImage> : ComponentBase, IImageComponent<TImage>
        where TImage : ImageSource
    {
        public ImageComponentBase(string name) : base(name)
        {
        }

        public IResourceLoader ResourceLoader { get; protected set; }

        public string ResourceKey { get; set; }

        public TImage Texture { get; protected set; }

        public void Load(string resourceKey)
        {
            if(string.IsNullOrEmpty(resourceKey))
                throw new ArgumentNullException(nameof(resourceKey));

            Texture = ResourceLoader.Load<TImage>(resourceKey);
            ResourceKey = resourceKey;
        }
    }
}
