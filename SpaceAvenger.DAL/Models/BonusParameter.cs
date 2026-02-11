using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class BonusParameter : EntityBase
    {
        public string ParameterName { get; set; }
        public string Unit { get; set; }

        public Bonus Bonus { get; set; }
        public int BonusId { get; set; }
    }
}
