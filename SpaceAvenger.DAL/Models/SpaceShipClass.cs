using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class SpaceShipClass : LoreEntity
    {
        public string ShipClass { get; set; }

        public ICollection<SpaceShip> SpaceShips { get; set; }
            = new List<SpaceShip>();

        public Faction Faction { get; set; }
        public int FactionId { get; set; }

        public Armor? Armor { get; set; }
        public int? ArmorId { get; set; }
    }
}
