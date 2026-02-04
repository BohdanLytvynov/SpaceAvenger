using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    //Faction is an organization
    //-> There can be lots of commanders with different ranks in 1 organization
    public class Faction : EntityBase
    {
        public string NameKey { get; set; }
        public string ShortNameKey { get; set; }
        public string ShortDescriptionKey { get; set; }
        public string DescriptionKey { get; set; }
        public ICollection<Commander> Commanders { get; set; } = new List<Commander>();
        public ICollection<StarFleetRank> Ranks { get; set; } = new List<StarFleetRank>();
    }
}
