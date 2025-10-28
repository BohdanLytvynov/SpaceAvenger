using WPFGameEngine.WPF.GE.Dto.Base;

namespace WPFGameEngine.WPF.GE.Dto.GameObject
{
    public class GameObjectDto : DtoBase
    {


        public GameObjectDto() : base(nameof(GameObjectDto))
        {
            
        }

        public GameObjectDto(string typeName) : base(typeName)
        {
            
        }
    }
}
