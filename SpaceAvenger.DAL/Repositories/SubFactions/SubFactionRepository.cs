using Microsoft.EntityFrameworkCore;
using SpaceAvenger.DAL.Models;
using SpaceAvenger.DAL.Repositories.Base;

namespace SpaceAvenger.DAL.Repositories.SubFactions
{
    public class SubFactionRepository : RepositoryBase<SubFaction>, ISubFactionRepository
    {
        public IEnumerable<SubFaction> GetSubFactions(int factionId)
        {
            return GetAll().Include(x => x.Faction).Where(x => x.FactionId == factionId).AsNoTracking();
        }
    }
}
