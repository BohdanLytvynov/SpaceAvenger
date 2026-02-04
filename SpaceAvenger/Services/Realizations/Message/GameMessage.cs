using System.Reflection.Metadata;
using ViewModelBaseLibDotNetCore.Message.Base;
using WPFGameEngine.WPF.GE.Levels;

namespace SpaceAvenger.Services.Realizations.Message
{
    internal class GameMessage : Message<string>
    {
        public ILevel Level { get; set; }

        public GameMessage(string content) : base(content)
        {
        }
    }
}
