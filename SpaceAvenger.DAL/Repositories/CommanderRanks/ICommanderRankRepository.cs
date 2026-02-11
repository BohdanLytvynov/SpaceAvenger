using SpaceAvenger.DAL.Models;
using SpaceAvenger.DAL.Repositories.Base;

namespace SpaceAvenger.DAL.Repositories.CommanderRanks
{
    public interface ICommanderRankRepository : IRepositoryBase<CommanderRank>
    {
        StarFleetRank? GetCommanderRank(int commanderId);

        int SetCommanderRank(Commander commander, StarFleetRank rank, Faction faction);
    }
}
