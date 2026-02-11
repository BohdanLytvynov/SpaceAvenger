using SpaceAvenger.DAL.Models.Base;
using SpaceAvenger.DAL.Models.Enums;

namespace SpaceAvenger.DAL.Models
{
    public class Planet : LoreEntity
    {
        public PlanetStatus Status { get; set; }
        public Faction? Faction { get; set; }
        public int? FactionId { get; set; }

        public float Population { get; set; }

        public ICollection<FactionHomeWorld> FactionHomePlanets { get; set; } 
            = new List<FactionHomeWorld>();
    }
}
