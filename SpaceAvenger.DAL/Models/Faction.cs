using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    //Faction is an organization
    //-> There can be lots of commanders with different ranks in 1 organization
    public class Faction : LoreEntity
    {
        public string FactionCode { get; set; }
        public string ShortNameKey { get; set; }
        public string ShortDescriptionKey { get; set; }
        public string ShipPrefix { get; set; }

        public ICollection<Commander> Commanders { get; set; } = new List<Commander>();
        public ICollection<StarFleetRank> Ranks { get; set; } = new List<StarFleetRank>();
        public ICollection<Planet> Planets { get; set; } = new List<Planet>();
        public ICollection<SpaceShip> SpaceShips { get; set; } = new List<SpaceShip>();
        public ICollection<SpaceShipClass> SpaceShipClasses { get; set; } = new List<SpaceShipClass>();

        public ICollection<FactionHomeWorlds> FactionHomeWorlds { get; set; } 
            = new List<FactionHomeWorlds>();
        public ICollection<FactionBonus> FactionBonuses { get; set; }
            = new List<FactionBonus>();
        public ICollection<FactionCurrency> FactionCurrency { get; set; }
            = new List<FactionCurrency>();
    }
}
