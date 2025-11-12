namespace WPFGameEngine.WPF.GE.Textures.Base
{
    public interface ITexture2D<TObject>
    {
        TObject Texture { get; }
        public double Width { get; }
        public double Height { get; }
        public double PixelWidth { get; }
        public double PixelHeight { get; }
    }
}
