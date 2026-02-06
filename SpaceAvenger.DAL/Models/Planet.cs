using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class Planet : LoreEntity
    {
        public Faction? Faction { get; set; }
        public int? FactionId { get; set; }

        public ICollection<FactionHomeWorlds> FactionHomePlanets { get; set; } 
            = new List<FactionHomeWorlds>();
    }
}
