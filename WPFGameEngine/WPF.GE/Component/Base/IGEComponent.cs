using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Dto.Base;

namespace WPFGameEngine.WPF.GE.Component.Base
{
    public interface IGEComponent : IGameEngineEntity, IConvertToDto<DtoBase>, ICloneable
    {
        /// <summary>
        /// List of components that are not compatible with the current one
        /// </summary>
        public List<string> IncompatibleComponents { get; }
        /// <summary>
        /// Name of the component use nameof(Component) for correct set up
        /// </summary>
        string ComponentName { get; init; }
    }
}
