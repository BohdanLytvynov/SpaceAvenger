using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class CommanderBonus : EntityBase
    {
        public Commander Commander { get; set; }
        public int? CommanderId { get; set; }

        public Bonus Bonus { get; set; }
        public int? BonusId { get; set; }
    }
}
