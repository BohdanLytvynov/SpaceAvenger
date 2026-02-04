using Microsoft.EntityFrameworkCore;
using SpaceAvenger.DAL.Models;
using SpaceAvenger.DAL.Repositories.Base;

namespace SpaceAvenger.DAL.Repositories.Commanders
{
    public class CommanderRepository : RepositoryBase<Commander>, ICommanderRepository
    {
        public CommanderRepository() : base()
        {
        }
    }
}
