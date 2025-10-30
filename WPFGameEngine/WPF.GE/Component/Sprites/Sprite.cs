using System.Windows.Media;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.WPF.GE.Component.Base.ImageComponents;
using WPFGameEngine.WPF.GE.Dto.Base;
using WPFGameEngine.WPF.GE.Dto.Components;

namespace WPFGameEngine.WPF.GE.Component.Sprites
{
    [VisibleInEditor(FactoryName = nameof(Sprite),
        DisplayName = "Sprite",
        GameObjectType = Enums.GEObjectType.Component)]
    public class Sprite : ImageComponentBase<ImageSource>, 
        IConvertToDto<SpriteDto>
    {
        #region Ctor
        
        public Sprite(IResourceLoader resourceLoader) : base(nameof(Sprite))
        {
            ResourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
        }
        #endregion

        #region Methods
        
        #region IConvertToDto

        public SpriteDto ToDto() =>
            new SpriteDto()
            {
                ResourceKey = ResourceKey,
            };

        #endregion

        #endregion

    }
}
