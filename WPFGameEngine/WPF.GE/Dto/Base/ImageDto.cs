namespace WPFGameEngine.WPF.GE.Dto.Base
{
    public abstract class ImageDto : DtoBase
    {
        public string ResourceKey { get; set; }

        protected ImageDto(string typeName)
        {
        }
    }
}
