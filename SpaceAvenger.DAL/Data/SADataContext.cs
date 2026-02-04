using Microsoft.EntityFrameworkCore;
using SpaceAvenger.DAL.Constants;
using SpaceAvenger.DAL.Models;
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
        public DbSet<Faction> Factions { get; set; }
        public DbSet<StarFleetRank> Ranks { get; set; }
        #endregion

        #region Ctor
        public SADataContext(string conString) : base()
        {
            m_conStr = conString ?? throw new ArgumentNullException(nameof(conString));

            if(string.IsNullOrEmpty(m_conStr))
                throw new ArgumentNullException(nameof(conString));
        }
        #endregion

        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(m_conStr);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(x => x.Commander)
                .WithOne(x => x.User)
                .HasForeignKey<Commander>(x=>x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Commander>()
                .HasOne(x => x.Faction)
                .WithMany(x => x.Commanders)
                .HasForeignKey(x => x.FactionId)
                .OnDelete(DeleteBehavior.NoAction);//Weak ref for dictionary - tables

            modelBuilder.Entity<Commander>()
                .HasOne(x => x.Rank)
                .WithMany(x => x.Commanders)
                .HasForeignKey(x => x.RankId)
                .OnDelete(DeleteBehavior.NoAction);//Weak ref for dictionary - tables

            SetUpDatabase(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SetUpDatabase(ModelBuilder modelBuilder)
        {
            InitFactions(modelBuilder);
            InitRanks(modelBuilder);
        }

        private void InitFactions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faction>().HasData(
                new Faction()
                {
                    Id = 1,
                    NameKey = BuildKey("UEF", SA_DALConstants.FullNameKey),
                    ShortNameKey = BuildKey("UEF", SA_DALConstants.ShortNameKey),
                    ShortDescriptionKey = BuildKey("UEF", SA_DALConstants.ShortDescKey),
                    DescriptionKey = BuildKey("UEF", SA_DALConstants.DescKey)
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
                        LevelNameKey = "UEF_L1", 
                        SortOrder = 1, MinExperience = 0,
                        RankType = "f", DescriptionKey = "UEF_L1_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 2,
                        FactionId = 1,
                        LevelNameKey = "UEF_L2",
                        SortOrder = 2, MinExperience = 1000, RankType = "f",
                        DescriptionKey = "UEF_L2_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 3,
                        FactionId = 1,
                        LevelNameKey = "UEF_L3", 
                        SortOrder = 3, MinExperience = 2500, RankType = "f",
                        DescriptionKey = "UEF_L3_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 4,
                        FactionId = 1,
                        LevelNameKey = "UEF_L4",
                        SortOrder = 4, MinExperience = 5000,
                        RankType = "f",
                        DescriptionKey = "UEF_L4_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 5,
                        FactionId = 1,
                        LevelNameKey = "UEF_L5",
                        SortOrder = 5, MinExperience = 9000,
                        RankType = "f",
                        DescriptionKey = "UEF_L5_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 6,
                        FactionId = 1,
                        LevelNameKey = "UEF_L6",
                        SortOrder = 6, MinExperience = 15000,
                        RankType = "f",
                        DescriptionKey = "UEF_L6_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 7,
                        FactionId = 1,
                        LevelNameKey = "UEF_L7",
                        SortOrder = 7, MinExperience = 25000,
                        RankType = "f",
                        DescriptionKey = "UEF_L7_Rank_Desc"
                    },
                    new StarFleetRank()
                    {
                        Id = 8,
                        FactionId = 1,
                        LevelNameKey = "UEF_L8",
                        SortOrder = 8, MinExperience = 45000,
                        RankType = "f",
                        DescriptionKey = "UEF_L8_Rank_Desc"


                    },
                    new StarFleetRank()
                    {
                        Id = 9,
                        FactionId = 1,
                        LevelNameKey = "UEF_L9",
                        SortOrder = 9, MinExperience = 75000,
                        RankType = "f",
                        DescriptionKey = "UEF_L9_Rank_Desc"
                    },
                    new StarFleetRank() 
                    {
                        Id = 10,
                        FactionId = 1,
                        LevelNameKey = "UEF_L10",
                        SortOrder = 10, MinExperience = 120000, RankType = "f",
                        DescriptionKey = "UEF_L10_Rank_Desc"
                    },
                    new StarFleetRank() 
                    {
                        Id = 11,
                        FactionId = 1,
                        LevelNameKey = "UEF_L11",
                        SortOrder = 11, MinExperience = 200000,
                        RankType = "f", DescriptionKey = "UEF_L11_Rank_Desc"
                    },
                    new StarFleetRank() 
                    {
                        Id = 12,
                        FactionId = 1,
                        LevelNameKey = "UEF_L12",
                        SortOrder = 12, MinExperience = 350000,
                        RankType = "f",
                        DescriptionKey = "UEF_L12_Rank_Desc"
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
