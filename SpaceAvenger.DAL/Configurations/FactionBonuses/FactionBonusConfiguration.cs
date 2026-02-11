using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceAvenger.DAL.Models;

namespace SpaceAvenger.DAL.Configurations.FactionBonuses
{
    public class FactionBonusConfiguration : IEntityTypeConfiguration<FactionBonus>
    {
        public void Configure(EntityTypeBuilder<FactionBonus> builder)
        {
            builder
                .HasKey(x => new { x.FactionId, x.BonusId });

            builder
                .HasOne(fb => fb.Faction)
                .WithMany(f => f.FactionBonuses)
                .HasForeignKey(fb => fb.FactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(fb => fb.Bonus)
                .WithMany(b => b.FactionBonuses)
                .HasForeignKey(fb => fb.BonusId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
