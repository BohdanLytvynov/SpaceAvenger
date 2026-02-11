using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class FactionBonus
    {
        public Bonus? Bonus { get; set; }
        public int? BonusId { get; set; }

        public Faction? Faction { get; set; }
        public int FactionId { get; set; }
    }
}
