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
        #region Fields
        private ImageSource? m_img;
       
        #endregion

        #region Properties
        public ImageSource Image { get => m_img; set => m_img = value; }
      
        #endregion

        #region Ctor
        
        public Sprite(IResourceLoader resourceLoader) : base(nameof(Sprite))
        {
            ResourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
            m_img = null;
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
