using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGameEngine.WPF.GE.Dto.GameObjects;
using WPFGameEngine.WPF.GE.Serialization.Base;

namespace WPFGameEngine.WPF.GE.Serialization.GameObjects
{
    public interface IGameObjectImporter : IObjectImporter<GameObjectDto>
    {
    }
}
