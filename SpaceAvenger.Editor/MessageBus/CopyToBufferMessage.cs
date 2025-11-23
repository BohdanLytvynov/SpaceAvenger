using ViewModelBaseLibDotNetCore.Message.Base;
using WPFGameEngine.Factories.Base;

namespace SpaceAvenger.Editor.MessageBus
{
    internal class CopyToBufferMessage : Message<IGameEngineEntity>
    {
        public CopyToBufferMessage(IGameEngineEntity content) : base(content)
        {
        }
    }

    internal class PasteFromBuffer : Message<IGameEngineEntity>
    {
        public PasteFromBuffer(IGameEngineEntity content) : base(content)
        {
        }
    }
}
