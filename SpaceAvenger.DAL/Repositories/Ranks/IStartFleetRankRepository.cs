using SpaceAvenger.DAL.Models;
using SpaceAvenger.DAL.Repositories.Base;

namespace SpaceAvenger.DAL.Repositories.Ranks
{
    public interface IStartFleetRankRepository : IRepositoryBase<StarFleetRank>
    {
        StarFleetRank? GetLowest();
    }
}
