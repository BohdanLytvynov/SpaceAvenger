namespace WPFGameEngine.WPF.GE.Dto.Base
{
    public interface IConvertToDto<TDto> 
        where TDto : DtoBase
    {
        TDto ToDto();
    }
}
