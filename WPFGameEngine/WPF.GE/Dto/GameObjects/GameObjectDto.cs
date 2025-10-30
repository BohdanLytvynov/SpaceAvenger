using WPFGameEngine.WPF.GE.Dto.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.WPF.GE.Dto.GameObjects
{
    public class GameObjectDto : DtoBase
    {


        public GameObjectDto() : base(nameof(GameObject))
        {
            
        }

        public GameObjectDto(string typeName) : base(typeName)
        {
            
        }
    }
}
