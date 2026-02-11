using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class CommanderRank : EntityBase
    {
        public StarFleetRank StarFleetRank { get; set; }
        public int? StarFleetRankId { get; set; }

        public Commander Commander { get; set; }
        public int CommanderId { get; set; }

        public Faction Faction { get; set; }
        public int? FactionId { get; set; }
    }
}
