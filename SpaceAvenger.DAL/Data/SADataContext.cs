using Microsoft.EntityFrameworkCore;
using SpaceAvenger.DAL.Constants;
using SpaceAvenger.DAL.Models;
using System.Reflection;
using System.Text;

namespace SpaceAvenger.DAL.Data
{
    public class SADataContext : DbContext
    {
        #region Fields
        private string m_conStr;
        #endregion

        #region Properties
        public DbSet<User> Users { get; set; }
        public DbSet<Commander> Commanders { get; set; }
        public DbSet<CommanderRank> CommanderRanks { get; set; }
        public DbSet<CommanderBonus> CommanderBonus { get; set; }
        public DbSet<Faction> Factions { get; set; }
        public DbSet<StarFleetRank> Ranks { get; set; }
        public DbSet<Bonus> Bonuses { get; set; }
        public DbSet<BonusParameter> BonusParameters { get; set; }
        public DbSet<CommanderWallet> CommanderWalet { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<FactionCurrency> FactionCurrencies { get; set; }
        public DbSet<FactionBonus> FactionBonuses { get; set; }
        public DbSet<FactionHomeWorld> FactionHomeWorlds { get; set; }
        public DbSet<Planet> Planets { get; set; }
        public DbSet<Projectile> Projectiles { get; set; }
        public DbSet<SpaceShip> SpaceShips { get; set; }
        public DbSet<SpaceShipClass> SpaceShipClasses { get; set; }
        public DbSet<SubFaction> SubFactions { get; set; }
        public DbSet<SubFactionBonus> SubFactionBonuses { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Armor> Armors { get; set; }
        #endregion

        #region Ctor
        public SADataContext(string conString) : base()
        {
            m_conStr = conString ?? throw new ArgumentNullException(nameof(conString));

            if (string.IsNullOrEmpty(m_conStr))
                throw new ArgumentNullException(nameof(conString));
        }
        #endregion

        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(m_conStr);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(User)));

            SetUpDatabase(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SetUpDatabase(ModelBuilder modelBuilder)
        {
            InitFactions(modelBuilder);
            InitSubFactions(modelBuilder);
            InitRanks(modelBuilder);
            InitPlanets(modelBuilder);
        }

        private void InitPlanets(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Planet>()
                .HasData(
                new Planet()//UEF
                {
                    Id = 1,
                    FactionId = 1,
                    NameKey = BuildKey("P_F10_0_", SA_DALConstants.FullNameKey),
                    DescriptionKey = BuildKey("P_F10_0_", SA_DALConstants.DescKey)
                }
                );
        }

        private void InitBonusParameter(ModelBuilder modelBuilder)
        {
           
        }

        private void InitBonuses(ModelBuilder modelBuilder)
        {
            //BonusParameter: What will be modified(InternalCode: "SPEED").
            //Bonus: Amount of the modification(ModifierValue: 1.15).
            //SubFactionBonus: SubFaction name(SubFactionId: 1 [NAA]).

            //UEF_NAA
        }

        private void InitSubFactions(ModelBuilder modelBuilder)
        {
            //ForUEF
            modelBuilder.Entity<SubFaction>()
                .HasData(
                new SubFaction()
                { 
                    Id = 1,
                    FactionId = 1,
                    SubFactionCode = "F10_NAA",
                    NameKey = BuildKey("F10_NAA", SA_DALConstants.FullNameKey),
                    ShortNameKey = BuildKey("F10_NAA", SA_DALConstants.ShortNameKey),
                    ShortDescriptionKey = BuildKey("F10_NAA", SA_DALConstants.ShortDescKey),
                    DescriptionKey = BuildKey("F10_NAA", SA_DALConstants.DescKey),
                    ImageName = "F10_SF1"
                },
                new SubFaction()
                {
                    Id = 2,
                    FactionId = 1,
                    SubFactionCode = "F10_ECU",
                    NameKey = BuildKey("F10_ECU", SA_DALConstants.FullNameKey),
                    ShortNameKey = BuildKey("F10_ECU", SA_DALConstants.ShortNameKey),
                    ShortDescriptionKey = BuildKey("F10_ECU", SA_DALConstants.ShortDescKey),
                    DescriptionKey = BuildKey("F10_ECU", SA_DALConstants.DescKey),
                    ImageName = "F10_SF2"
                },
                new SubFaction()
                {
                    Id = 3,
                    FactionId = 1,
                    SubFactionCode = "F10_PAS",
                    NameKey = BuildKey("F10_PAS", SA_DALConstants.FullNameKey),
                    ShortNameKey = BuildKey("F10_PAS", SA_DALConstants.ShortNameKey),
                    ShortDescriptionKey = BuildKey("F10_PAS", SA_DALConstants.ShortDescKey),
                    DescriptionKey = BuildKey("F10_PAS", SA_DALConstants.DescKey),
                    ImageName = "F10_SF3"
                },
                new SubFaction()
                {
                    Id = 4,
                    FactionId = 1,
                    SubFactionCode = "F10_STC",
                    NameKey = BuildKey("F10_STC", SA_DALConstants.FullNameKey),
                    ShortNameKey = BuildKey("F10_STC", SA_DALConstants.ShortNameKey),
                    ShortDescriptionKey = BuildKey("F10_STC", SA_DALConstants.ShortDescKey),
                    DescriptionKey = BuildKey("F10_STC", SA_DALConstants.DescKey),
                    ImageName = "F10_SF4"
                }
                );
        }

        private void InitFactions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faction>().HasData(
                new Faction()//UEF
                {
                    Id = 1,
                    FactionCode = "F10",
                    NameKey = BuildKey("F10", SA_DALConstants.FullNameKey),
                    ShortNameKey = BuildKey("F10", SA_DALConstants.ShortNameKey),
                    ShortDescriptionKey = BuildKey("F10", SA_DALConstants.ShortDescKey),
                    DescriptionKey = BuildKey("F10", SA_DALConstants.DescKey),
                    ShipPrefix = BuildKey("F10", SA_DALConstants.ShipPrefixKey),
                    ImageName = "F10_Icon"
                },
                new Faction()
                {
                    Id = 2,
                    NameKey = BuildKey("F1", SA_DALConstants.FullNameKey),
                    ShortNameKey = BuildKey("F1", SA_DALConstants.ShortNameKey),
                    ShortDescriptionKey = BuildKey("F1", SA_DALConstants.ShortDescKey),
                    DescriptionKey = BuildKey("F1", SA_DALConstants.DescKey),
                    FactionCode = "F1",
                    ImageName = "F1_Icon"
                }
                );
        }

        private void InitRanks(ModelBuilder modelBuilder)
        {
            //UEF Ranks
            modelBuilder.Entity<StarFleetRank>().HasData
                (
                    new StarFleetRank()
                    {
                        Id = 1,
                        FactionId = 1,
                        LevelNameKey = "F10_L1",
                        SortOrder = 1,
                        MinExperience = 0,
                        RankType = "f",
                        DescriptionKey = "F10_L1_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 2,
                        FactionId = 1,
                        LevelNameKey = "F10_L2",
                        SortOrder = 2,
                        MinExperience = 1000,
                        RankType = "f",
                        DescriptionKey = "F10_L2_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 3,
                        FactionId = 1,
                        LevelNameKey = "F10_L3",
                        SortOrder = 3,
                        MinExperience = 2500,
                        RankType = "f",
                        DescriptionKey = "F10_L3_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 4,
                        FactionId = 1,
                        LevelNameKey = "F10_L4",
                        SortOrder = 4,
                        MinExperience = 5000,
                        RankType = "f",
                        DescriptionKey = "F10_L4_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 5,
                        FactionId = 1,
                        LevelNameKey = "F10_L5",
                        SortOrder = 5,
                        MinExperience = 9000,
                        RankType = "f",
                        DescriptionKey = "F10_L5_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 6,
                        FactionId = 1,
                        LevelNameKey = "F10_L6",
                        SortOrder = 6,
                        MinExperience = 15000,
                        RankType = "f",
                        DescriptionKey = "F10_L6_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 7,
                        FactionId = 1,
                        LevelNameKey = "F10_L7",
                        SortOrder = 7,
                        MinExperience = 25000,
                        RankType = "f",
                        DescriptionKey = "F10_L7_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 8,
                        FactionId = 1,
                        LevelNameKey = "F10_L8",
                        SortOrder = 8,
                        MinExperience = 45000,
                        RankType = "f",
                        DescriptionKey = "F10_L8_Rank_Desc"


                    },
                    new StarFleetRank()
                    {
                        Id = 9,
                        FactionId = 1,
                        LevelNameKey = "F10_L9",
                        SortOrder = 9,
                        MinExperience = 75000,
                        RankType = "f",
                        DescriptionKey = "F10_L9_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 10,
                        FactionId = 1,
                        LevelNameKey = "F10_L10",
                        SortOrder = 10,
                        MinExperience = 120000,
                        RankType = "f",
                        DescriptionKey = "F10_L10_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 11,
                        FactionId = 1,
                        LevelNameKey = "F10_L11",
                        SortOrder = 11,
                        MinExperience = 200000,
                        RankType = "f",
                        DescriptionKey = "F10_L11_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 12,
                        FactionId = 1,
                        LevelNameKey = "F10_L12",
                        SortOrder = 12,
                        MinExperience = 350000,
                        RankType = "f",
                        DescriptionKey = "F10_L12_Rank_Desc"
                    }
                );
        }

        private string BuildKey(string arg0, string arg1)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(arg0);
            sb.Append(arg1);
            return sb.ToString();
        }
        #endregion
    }
}
