using Microsoft.EntityFrameworkCore;
using SpaceAvenger.DAL.Models;
using SpaceAvenger.DAL.Repositories.Base;

namespace SpaceAvenger.DAL.Repositories.Factions
{
    public class FactionRepository : RepositoryBase<Faction>, IFactionRepository
    {
        public FactionRepository() : base()
        {
        }

        public Faction GetUEF() =>
            GetAll().FirstOrDefault(c => c.Id == 1);
    }
}
