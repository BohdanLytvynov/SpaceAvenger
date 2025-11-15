using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Controllers;
using WPFGameEngine.WPF.GE.Dto.Base;

namespace WPFGameEngine.WPF.GE.Dto.Components
{
    public class ControllerDto : ComponentDto
    {
        public ControllerDto()
        {
            
        }

        public override IGEComponent ToObject(IFactoryWrapper factoryWrapper)
        {
            throw new NotImplementedException();//Not used in Editor
        }
    }
}
