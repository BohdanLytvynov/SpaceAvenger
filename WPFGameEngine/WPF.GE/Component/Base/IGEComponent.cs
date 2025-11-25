using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Dto.Base;

namespace WPFGameEngine.WPF.GE.Component.Base
{
    public interface IGEComponent : IGameEngineEntity, IConvertToDto<DtoBase>, ICloneable
    {
        public List<string> IncompatibleComponents { get; }
        string ComponentName { get; init; }
    }
}
