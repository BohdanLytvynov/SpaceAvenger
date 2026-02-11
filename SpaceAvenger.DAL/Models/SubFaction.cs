using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class SubFaction : LoreEntity
    {
        public string SubFactionCode { get; set; }
        public string ShortNameKey { get; set; }
        public string ShortDescriptionKey { get; set; }

        public Faction Faction { get; set; }
        public int FactionId { get; set; }

        public ICollection<Commander> Commanders { get; set; }
            = new List<Commander>();

        public ICollection<SubFactionBonus> SubFactionBonuses { get; set; }
            = new List<SubFactionBonus>();

        public ICollection<Weapon> Weapons { get; set; }
           = new List<Weapon>();

        public ICollection<Projectile> Projectiles { get; set; }
            = new List<Projectile>();

        public ICollection<Armor> Armors { get; set; }
            = new List<Armor>();
    }
}
