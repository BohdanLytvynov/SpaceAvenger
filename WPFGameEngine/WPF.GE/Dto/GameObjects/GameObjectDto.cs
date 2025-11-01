using WPFGameEngine.WPF.GE.Dto.Base;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.WPF.GE.Dto.GameObjects
{
    public class GameObjectDto : DtoBase
    {
        public double ZIndex { get; set; }
        public  bool Enabled { get; set; }
        public string Name { get; set; }
        public List<GameObjectDto> Children { get; set; }
        public List<DtoBase> Components { get; set; }

        public GameObjectDto()
        {
            Init();
        }

        private void Init()
        { 
            Children = new List<GameObjectDto>();
            Components = new List<DtoBase>();
        }
    }
}
