using SpaceAvenger.DAL.Models;
using SpaceAvenger.DAL.Repositories.Base;

namespace SpaceAvenger.DAL.Repositories.SubFactions
{
    public interface ISubFactionRepository : IRepositoryBase<SubFaction>
    {
        IEnumerable<SubFaction> GetSubFactions(int factionId);
    }
}
