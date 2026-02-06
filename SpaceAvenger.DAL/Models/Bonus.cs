using SpaceAvenger.DAL.Models.Base;
using SpaceAvenger.DAL.Models.Enums;

namespace SpaceAvenger.DAL.Models
{
    public class Bonus : LoreEntity
    {
        public BonusType Type { get; set; }
        public float ModifierValue { get; set; }
        public bool IsPercentage { get; set; }

        public ICollection<FactionBonus> FactionBonuses { get; set; } 
            = new List<FactionBonus>();
    }
}
