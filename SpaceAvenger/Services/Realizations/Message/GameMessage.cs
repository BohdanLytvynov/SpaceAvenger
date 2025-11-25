using ViewModelBaseLibDotNetCore.Message.Base;

namespace SpaceAvenger.Services.Realizations.Message
{
    internal class GameMessage : Message<string>
    {
        public GameMessage(string content) : base(content)
        {
        }
    }
}
