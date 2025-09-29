using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceAvenger.Services.Realizations.Message
{
    internal class GameMessage : Message<string>
    {
        public GameMessage(string content) : base(content)
        {
        }
    }
}
