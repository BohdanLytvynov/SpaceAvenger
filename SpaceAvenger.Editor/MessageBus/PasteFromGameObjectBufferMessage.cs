using SpaceAvenger.Editor.Mock;
using ViewModelBaseLibDotNetCore.Message.Base;

namespace SpaceAvenger.Editor.MessageBus
{
    internal class PasteFromGameObjectBufferMessage : Message<IGameObjectMock>
    {
        public PasteFromGameObjectBufferMessage(IGameObjectMock content) : base(content)
        {
        }
    }
}
