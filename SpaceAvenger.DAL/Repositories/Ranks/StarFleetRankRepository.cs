using Microsoft.EntityFrameworkCore;
using SpaceAvenger.DAL.Models;
using SpaceAvenger.DAL.Repositories.Base;

namespace SpaceAvenger.DAL.Repositories.Ranks
{
    public class StarFleetRankRepository : RepositoryBase<StarFleetRank>, 
        IStartFleetRankRepository
    {
        public StarFleetRankRepository() : base()
        {
        }

        public StarFleetRank? GetLowest(Faction faction)
        {
            if (faction == null) throw new ArgumentNullException(nameof(faction));

            return GetAll().Include(x => x.Faction).Where(x => x.FactionId == faction.Id)
            .OrderBy(c => c.SortOrder).FirstOrDefault();
        }
    }
}
