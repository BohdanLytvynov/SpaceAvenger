using WPFGameEngine.WPF.GE.Dto.Base;

namespace WPFGameEngine.WPF.GE.Component.Base
{
    public abstract class ComponentBase : IGEComponent
    {
        public string ComponentName { get; init; }

        public ComponentBase(string name)
        {
            ComponentName = name;
        }

        public abstract DtoBase ToDto();
    }
}
