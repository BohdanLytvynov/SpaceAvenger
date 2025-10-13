using System.Windows;
using System.Windows.Media;
using WPFGameEngine.WPF.GE.Component.Base;

namespace WPFGameEngine.WPF.GE.Component.Sprites
{
    public interface ISprite : IComponent
    {
        public ImageSource Image { get; set; }

        void Load(ResourceDictionary resourceDictionary, string resourceName);
    }
}
