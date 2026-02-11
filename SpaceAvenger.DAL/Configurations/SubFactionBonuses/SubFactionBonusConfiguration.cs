using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceAvenger.DAL.Models;

namespace SpaceAvenger.DAL.Configurations.SubFactionBonuses
{
    public class SubFactionBonusConfiguration : IEntityTypeConfiguration<SubFactionBonus>
    {
        public void Configure(EntityTypeBuilder<SubFactionBonus> builder)
        {
            builder
               .HasKey(x => new { x.SubFactionId, x.BonusId });

            builder
                .HasOne(x => x.SubFaction)
                .WithMany(x => x.SubFactionBonuses)
                .HasForeignKey(x => x.SubFactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.Bonus)
                .WithMany(x => x.SubFactionBonuses)
                .HasForeignKey(x => x.BonusId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
