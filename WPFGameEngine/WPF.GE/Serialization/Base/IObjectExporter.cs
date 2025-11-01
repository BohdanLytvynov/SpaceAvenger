using WPFGameEngine.WPF.GE.Dto.Base;

namespace WPFGameEngine.WPF.GE.Serialization.Base
{
    public interface IObjectExporter<in TInput, in TDto>
        where TInput : IConvertToDto<TDto>
        where TDto : DtoBase
    {
        void Export(TInput inpObj, string path, Exception exception);
    }
}
