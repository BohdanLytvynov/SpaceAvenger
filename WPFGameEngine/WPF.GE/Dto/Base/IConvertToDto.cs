using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGameEngine.WPF.GE.Dto.Base
{
    internal interface IConvertToDto<TDto>
        where TDto : DtoBase
    {
        TDto ToDto();
    }
}
