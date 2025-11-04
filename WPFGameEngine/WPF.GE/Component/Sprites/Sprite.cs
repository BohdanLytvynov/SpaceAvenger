using System.Windows.Media;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Base.ImageComponents;
using WPFGameEngine.WPF.GE.Dto.Components;

namespace WPFGameEngine.WPF.GE.Component.Sprites
{
    [VisibleInEditor(FactoryName = nameof(Sprite),
        DisplayName = "Sprite",
        GameObjectType = Enums.GEObjectType.Component)]
    public class Sprite : ImageComponentBase<ImageSource>, ISprite
        
    {
        public override List<string> IncompatibleComponents => 
            new List<string> { nameof(Animation), nameof(Animator) };

        #region Ctor
        
        public Sprite(IResourceLoader resourceLoader) : base(nameof(Sprite))
        {
            ResourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
        }
        #endregion

        #region Methods
        
        #region IConvertToDto

        public override SpriteDto ToDto() =>
            new SpriteDto()
            {
                ResourceKey = ResourceKey,
            };

        #endregion

        #endregion

    }
}
