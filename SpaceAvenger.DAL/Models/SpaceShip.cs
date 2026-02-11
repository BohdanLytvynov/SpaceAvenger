using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class SpaceShip : LoreEntity
    {
        public SpaceShipClass SpaceShipClass { get; set; }
        public int SpaceShipClassId { get; set; }

        public Faction Faction { get; set; }
        public int FactionId { get; set; }

        public Commander? Commander { get; set; }
        public int? CommanderId { get; set; }

        public string MapableObject { get; set; }
    }
}
