using WPFGameEngine.Factories.Base;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.WPF.GE.Component.Sprites;

namespace WPFGameEngine.Factories.Components.Sprites
{
    public class SpriteFactory : FactoryBase, ISpriteFactory
    {
        public IResourceLoader ResourceLoader { get; protected set; }

        public SpriteFactory(IResourceLoader resourceLoader)
        {
            ResourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
            ProductName = nameof(Sprite);
        }

        public override Sprite Create()
        {
            return new Sprite(ResourceLoader);
        }
    }
}
