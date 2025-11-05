using WPFGameEngine.Factories.Base;
using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Dto.Base;

namespace WPFGameEngine.WPF.GE.Dto.Components
{
    public class SpriteDto : ImageDto
    {
        public SpriteDto()
        {

        }

        public override ISprite ToObject(IFactoryWrapper factoryWrapper)
        {
            var sprite = new Sprite(factoryWrapper.ResourceLoader)
            {
                ResourceKey = ResourceKey
            };
            sprite.Load(ResourceKey);

            return sprite;
        }
    }
}
