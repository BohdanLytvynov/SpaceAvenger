using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceAvenger.DAL.Models;

namespace SpaceAvenger.DAL.Configurations.CommanderRanks
{
    public class CommanderRankConfiguration : IEntityTypeConfiguration<CommanderRank>
    {
        public void Configure(EntityTypeBuilder<CommanderRank> builder)
        {
            builder
                .HasOne(x => x.Commander)
                .WithMany(x => x.CommanderRanks)
                .HasForeignKey(x => x.CommanderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.StarFleetRank)
                .WithMany(x => x.CommanderRanks)
                .HasForeignKey(x => x.StarFleetRankId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
