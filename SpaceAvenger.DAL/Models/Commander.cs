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
        public int RankId { get; set; }
        public StarFleetRank Rank { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int FactionId { get; set; }
        public Faction Faction { get; set; }
        #endregion
    }
}
