using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelBaseLibDotNetCore.Message.Base;
using WPFGameEngine.WPF.GE.Levels;

namespace SpaceAvenger.Services.Realizations.Message
{
    internal class LevelStatisticMessage : Message<LevelStatistics>
    {
        public LevelStatisticMessage(LevelStatistics content) : base(content)
        {
        }
    }
}
