using SpaceAvenger.Editor.ViewModels.Components.Base;
using ViewModelBaseLibDotNetCore.Message.Base;

namespace SpaceAvenger.Editor.MessageBus
{
    internal class PasteFromComponentsBufferMessage : Message<ComponentViewModel>
    {
        public PasteFromComponentsBufferMessage(ComponentViewModel content) : base(content)
        {
        }
    }
}
