using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class FactionHomeWorld
    {
        public Faction Faction { get; set; }
        public int FactionId { get; set; }

        public Planet HomePlanet { get; set; }
        public int HomePlanetId { get; set; }
    }
}
