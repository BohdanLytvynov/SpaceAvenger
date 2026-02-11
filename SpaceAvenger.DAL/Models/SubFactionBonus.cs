using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class SubFactionBonus : LoreEntity
    {
        public SubFaction SubFaction { get; set; }
        public int SubFactionId { get; set; }

        public Bonus Bonus { get; set; }
        public int BonusId { get; set; }
    }
}
