using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceAvenger.DAL.Models;

namespace SpaceAvenger.DAL.Configurations.StarFleetRanks
{
    public class StarFleetRankConfiguration : IEntityTypeConfiguration<StarFleetRank>
    {
        public void Configure(EntityTypeBuilder<StarFleetRank> builder)
        {
            
        }
    }
}
