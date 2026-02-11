using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class Weapon : LoreEntity
    {
        public Faction Faction { get; set; }
        public int FactionId { get; set; }

        public SubFaction SubFaction { get; set; }
        public int SubFactionId { get; set; }

        public bool Maintainable { get; set; }
    }
}
