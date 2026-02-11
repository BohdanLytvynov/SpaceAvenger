using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class Commander : EntityBase
    {
        #region Properties
        public bool MaleFemale { get; set; }
        public int MissionsCount { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Confirmed { get; set; }
        public float Points { get; set; }

        #endregion

        #region Navigation Properties

        public int UserId { get; set; }
        public User User { get; set; }

        public int? FactionId { get; set; }
        public Faction Faction { get; set; }

        public SubFaction? SubFaction { get; set; }
        public int? SubFactionId { get; set; }

        public ICollection<CommanderWallet> CommanderWallets { get; set; }
            = new List<CommanderWallet>();

        public ICollection<SpaceShip> SpaceShips { get; set; } 
            = new List<SpaceShip>();

        public ICollection<CommanderRank> CommanderRanks { get; set; }
            = new List<CommanderRank>();

        public ICollection<CommanderBonus> CommanderBonuses { get; set; }
            = new List<CommanderBonus>();
        #endregion
    }
}
