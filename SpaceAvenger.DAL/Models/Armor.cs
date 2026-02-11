using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class Armor : LoreEntity
    {
        public Faction Faction { get; set; }
        public int? FactionId { get; set; }

        public SubFaction? SubFaction { get; set; }
        public int? SubFactionId { get; set; }

        /// <summary>
        /// Kinetic damage resistance
        /// </summary>
        public float KineticResist { get; set; }
        /// <summary>
        /// Energy melting weapon resistance
        /// </summary>
        public float EnergyResist { get; set; }
        /// <summary>
        /// Projectiles with explosives warhead resistance
        /// </summary>
        public float ExplosiveResist { get; set; }

        public ICollection<SpaceShipClass> SpaceShipClasses { get; set; }
            = new List<SpaceShipClass>();
    }
}
