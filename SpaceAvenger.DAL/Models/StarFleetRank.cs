using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class StarFleetRank : EntityBase
    {
        public string LevelNameKey { get; set; }
        public int SortOrder { get; set; }
        public int MinExperience { get; set; }
        public string RankType { get; set; }//w,f,g
        public string DescriptionKey { get; set; }

        #region Navigation Properties
        public ICollection<CommanderRank> CommanderRanks { get; set; } 
            = new List<CommanderRank>();

        public int? FactionId { get; set; }
        public Faction Faction { get; set; }
        #endregion
    }
}
