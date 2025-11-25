using SpaceAvenger.Editor.ViewModels.Components.Base;
using ViewModelBaseLibDotNetCore.Message.Base;
using WPFGameEngine.WPF.GE.Component.Base;

namespace SpaceAvenger.Editor.MessageBus
{
    internal class PasteFromComponentsBufferMessage : Message<IGEComponent>
    {
        public PasteFromComponentsBufferMessage(IGEComponent content) : base(content)
        {
        }
    }
}
