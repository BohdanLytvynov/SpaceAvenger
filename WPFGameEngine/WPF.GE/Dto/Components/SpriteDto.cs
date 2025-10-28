using System.Windows.Media;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Dto.Base;

namespace WPFGameEngine.WPF.GE.Dto.Components
{
    public class SpriteDto : DtoBase, IConvertToComponent<Sprite, SpriteDto>
    {
        public string ResourceKey { get; set; }

        public SpriteDto() : base(nameof(Sprite))
        {
        }

        public IGEComponent ToComponent()
        {
            return null;
        }
    }
}
