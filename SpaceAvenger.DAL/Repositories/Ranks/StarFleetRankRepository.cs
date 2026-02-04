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

        public StarFleetRank? GetLowest() => 
            GetAll().OrderBy(c => c.SortOrder).FirstOrDefault();
    }
}
