using System.Text.Json.Serialization;
using WPFGameEngine.WPF.GE.Dto.Base;

namespace WPFGameEngine.WPF.GE.Component.Base
{
    public abstract class ComponentBase : IGEComponent
    {
        public string ComponentName { get; init; }

        [JsonIgnore]
        public abstract List<string> IncompatibleComponents { get; }

        public ComponentBase(string name)
        {
            ComponentName = name;
        }

        public abstract DtoBase ToDto();
    }
}
