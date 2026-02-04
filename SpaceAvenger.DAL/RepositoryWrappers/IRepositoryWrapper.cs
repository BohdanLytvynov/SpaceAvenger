using SpaceAvenger.DAL.Repositories.Commanders;
using SpaceAvenger.DAL.Repositories.Factions;
using SpaceAvenger.DAL.Repositories.Ranks;
using SpaceAvenger.DAL.Repositories.Users;

namespace SpaceAvenger.DAL.RepositoryWrappers
{
    public interface IRepositoryWrapper
    {
        IUserRepository UserRepository { get; }
        ICommanderRepository CommanderRepository { get; }
        IStartFleetRankRepository StarFleetRankRepository { get; }
        IFactionRepository FactionRepository { get; }
        int Save();
    }
}
