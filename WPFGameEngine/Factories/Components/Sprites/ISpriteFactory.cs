using WPFGameEngine.Factories.Base;
using WPFGameEngine.Services.Interfaces;
using SpriteComponent = WPFGameEngine.WPF.GE.Component.Sprites.Sprite;

namespace WPFGameEngine.Factories.Components.Sprites
{
    public interface ISpriteFactory : IFactory
    {
        IResourceLoader ResourceLoader { get; }
    }
}
