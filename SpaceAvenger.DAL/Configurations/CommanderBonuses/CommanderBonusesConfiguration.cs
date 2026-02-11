using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceAvenger.DAL.Models;

namespace SpaceAvenger.DAL.Configurations.CommanderBonuses
{
    public class CommanderBonusesConfiguration : IEntityTypeConfiguration<CommanderBonus>
    {
        public void Configure(EntityTypeBuilder<CommanderBonus> builder)
        {
            builder
                .HasOne(x => x.Commander)
                .WithMany(x => x.CommanderBonuses)
                .HasForeignKey(x => x.CommanderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.Bonus)
                .WithMany(x => x.CommanderBonuses)
                .HasForeignKey(x => x.BonusId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
