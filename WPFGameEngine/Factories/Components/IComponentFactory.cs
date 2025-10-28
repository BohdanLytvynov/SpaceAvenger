using WPFGameEngine.Factories.Base;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.WPF.GE.Component.Base;

namespace WPFGameEngine.Factories.Components
{
    public interface IComponentFactory : IAbstractFactory<IGEComponent>
    {
        public IResourceLoader ResourceLoader { get; }
    }
}
