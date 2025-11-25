using SpaceAvenger.Editor.ViewModels.Components.Base;
using ViewModelBaseLibDotNetCore.Message.Base;

namespace SpaceAvenger.Editor.MessageBus
{
    internal class CopyToComponentsBufferMessage : Message<ComponentViewModel>
    {
        public CopyToComponentsBufferMessage(ComponentViewModel content) : base(content)
        {
        }
    }
}
