using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class SpaceShipClass : LoreEntity
    {
        public string Class { get; set; }

        public SpaceShip SapceShip { get; set; }

        public Faction Faction { get; set; }
        public int FactionId { get; set; }
    }
}
