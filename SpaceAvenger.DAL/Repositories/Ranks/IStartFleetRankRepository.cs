using SpaceAvenger.DAL.Models;
using SpaceAvenger.DAL.Repositories.Base;

namespace SpaceAvenger.DAL.Repositories.Ranks
{
    public interface IStartFleetRankRepository : IRepositoryBase<StarFleetRank>
    {
        /// <summary>
        /// Gets the Lowest Rank of the Faction
        /// </summary>
        /// <param name="faction"></param>
        /// <returns></returns>
        StarFleetRank? GetLowest(Faction faction);
    }
}
