using Microsoft.EntityFrameworkCore;
using SpaceAvenger.DAL.Repositories.Base;
using SpaceAvenger.DAL.Repositories.CommanderRanks;
using SpaceAvenger.DAL.Repositories.Commanders;
using SpaceAvenger.DAL.Repositories.Factions;
using SpaceAvenger.DAL.Repositories.Ranks;
using SpaceAvenger.DAL.Repositories.Users;

namespace SpaceAvenger.DAL.RepositoryWrappers
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        #region Fields
        private IUserRepository? m_userRepository;
        private IStartFleetRankRepository? m_startFleetRankRepository;
        private ICommanderRepository? m_commanderRepository;
        private IFactionRepository? m_factionRepository;
        private ICommanderRankRepository m_commanderRankRepository;
        private DbContext m_dbContext;
        #endregion

        #region Properties
        public IUserRepository UserRepository
        {
            get => CreateLazy<IUserRepository, UserRepository>(ref m_userRepository);
        }

        public IStartFleetRankRepository StarFleetRankRepository
        {
            get => CreateLazy<IStartFleetRankRepository, StarFleetRankRepository>(ref m_startFleetRankRepository);
        }

        public ICommanderRepository CommanderRepository 
        {
            get => CreateLazy<ICommanderRepository, CommanderRepository>(ref m_commanderRepository);
        }

        public IFactionRepository FactionRepository 
        { 
            get => CreateLazy<IFactionRepository, FactionRepository>(ref m_factionRepository);
        }

        public ICommanderRankRepository CommanderRankRepository 
        { 
            get => CreateLazy<ICommanderRankRepository, CommanderRankRepository>(ref m_commanderRankRepository);
        }
        #endregion

        #region Ctor
        public RepositoryWrapper(DbContext dbContext)
        {
            m_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        #endregion

        #region Methods
        public int Save()
        { 
            return m_dbContext.SaveChanges();
        }

        private IRepo CreateLazy<IRepo, TRepo>(ref IRepo field)
            where IRepo : class, IDataContextContainer
            where TRepo : class, new()
        {
            if (field == null)
            {
                field = new TRepo() as IRepo ?? throw new InvalidOperationException($"Unable to create Repo <{nameof(TRepo)}>! Check if <{nameof(TRepo)}> implements <{nameof(IRepo)}>.");
                field.Context = m_dbContext;
            }

            return field;
        }
        #endregion
    }
}
