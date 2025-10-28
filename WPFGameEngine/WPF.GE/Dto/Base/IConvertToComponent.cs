using WPFGameEngine.WPF.GE.Component.Base;

namespace WPFGameEngine.WPF.GE.Dto.Base
{
    internal interface IConvertToComponent<TComponent, TDto>
        where TComponent : IGEComponent, IConvertToDto<TDto>
        where TDto : DtoBase
    {
        IGEComponent ToComponent();
    }
}
