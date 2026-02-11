using Microsoft.EntityFrameworkCore;
using SpaceAvenger.DAL.Models;
using SpaceAvenger.DAL.Repositories.Base;

namespace SpaceAvenger.DAL.Repositories.CommanderRanks
{
    public class CommanderRankRepository : RepositoryBase<CommanderRank>, ICommanderRankRepository
    {
        public StarFleetRank? GetCommanderRank(int commanderId)
        {
            return GetAll().Where(x => x.CommanderId == commanderId).Include(x => x.StarFleetRank)
                .Select(x => x.StarFleetRank).FirstOrDefault();
        }

        public int SetCommanderRank(Commander commander, StarFleetRank rank, Faction faction)
        {
            commander.CommanderRanks.Add(new CommanderRank()
            {
                Commander = commander,
                StarFleetRank = rank,
                Faction = faction
            });

            return Save();
        }
    }
}
