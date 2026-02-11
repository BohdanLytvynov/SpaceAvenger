using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class Bonus : EntityBase
    {
        public float ModifierValue { get; set; }
        public float Duration { get; set; }
        public bool IsPercentage { get; set; }
        public bool IsPositive { get; set; }

        public ICollection<BonusParameter> BonusParameters { get; set; }
        = new List<BonusParameter>();

        public ICollection<FactionBonus> FactionBonuses { get; set; } 
            = new List<FactionBonus>();

        public ICollection<SubFactionBonus> SubFactionBonuses { get; set; }
            = new List<SubFactionBonus>();

        public ICollection<CommanderBonus> CommanderBonuses { get; set; }
            = new List<CommanderBonus>();
    }
}
