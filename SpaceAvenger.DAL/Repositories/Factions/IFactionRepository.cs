using SpaceAvenger.DAL.Models;
using SpaceAvenger.DAL.Repositories.Base;

namespace SpaceAvenger.DAL.Repositories.Factions
{
    public interface IFactionRepository : IRepositoryBase<Faction>
    {
        Faction GetUEF();
    }
}
