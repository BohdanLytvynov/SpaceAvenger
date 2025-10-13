using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Exceptions;
using WPFGameEngine.WPF.GE.Helpers;

namespace WPFGameEngine.WPF.GE.Component.Sprites
{
    public class Sprite : ComponentBase, ISprite
    {
        #region Fields
        private ImageSource? m_img;
        #endregion

        #region Properties
        public ImageSource Image { get => m_img; set => m_img = value; }

        #endregion

        #region Ctor
        public Sprite(ImageSource img) : base(nameof(Sprite))
        {
            m_img = img ?? throw new ArgumentNullException(nameof(img));
        }

        public Sprite() : base(nameof(Sprite))
        {
            m_img = null;
        }
        #endregion

        #region Methods

        public void Load(ResourceDictionary resourceDictionary, string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
                throw new EmptyArgumentException(nameof(resourceName));

            if(resourceDictionary == null)
                throw new ArgumentNullException (nameof(resourceDictionary));

            var imgSrc = (ImageSource)resourceDictionary[resourceName];
            if(imgSrc == null)
                throw new ResourceGetException(nameof(resourceName));

            m_img = imgSrc;
        }

        #endregion

    }
}
