using WPFGameEngine.WPF.GE.Textures.Base;

namespace WPFGameEngine.WPF.GE.Textures
{
    public abstract class Texture2D<TObject> : ITexture2D<TObject>
    {
        public abstract double Width { get; protected set; }
        public abstract double Height { get; protected set; }
        public abstract double PixelWidth { get; protected set; }
        public abstract double PixelHeight { get; protected set; }
        public abstract TObject Texture { get; protected set; }
    }
}
