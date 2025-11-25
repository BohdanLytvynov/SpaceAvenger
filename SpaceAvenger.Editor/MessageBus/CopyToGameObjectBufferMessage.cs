using SpaceAvenger.Editor.ViewModels.TreeItems;
using ViewModelBaseLibDotNetCore.Message.Base;

namespace SpaceAvenger.Editor.MessageBus
{
    internal class CopyToGameObjectBufferMessage : Message<TreeItemViewModel>
    {
        public CopyToGameObjectBufferMessage(TreeItemViewModel content) : base(content)
        {
        }
    }
}
