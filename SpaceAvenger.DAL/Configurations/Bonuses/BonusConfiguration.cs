using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceAvenger.DAL.Models;

namespace SpaceAvenger.DAL.Configurations.Bonuses
{
    public class BonusConfiguration : IEntityTypeConfiguration<Bonus>
    {
        public void Configure(EntityTypeBuilder<Bonus> builder)
        {
            builder
                .Property(x => x.ModifierValue)
                .HasPrecision(18, 4);

            builder.
                HasMany(x => x.BonusParameters)
                .WithOne(x => x.Bonus)
                .HasForeignKey(x => x.BonusId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
