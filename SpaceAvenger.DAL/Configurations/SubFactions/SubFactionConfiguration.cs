using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceAvenger.DAL.Models;

namespace SpaceAvenger.DAL.Configurations.SubFactions
{
    public class SubFactionConfiguration : IEntityTypeConfiguration<SubFaction>
    {
        public void Configure(EntityTypeBuilder<SubFaction> builder)
        {
            builder
               .HasMany(x => x.Weapons)
               .WithOne(x => x.SubFaction)
               .HasForeignKey(x => x.SubFactionId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.Projectiles)
                .WithOne(x => x.SubFaction)
                .HasForeignKey(x => x.SubFactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.Armors)
                .WithOne(x => x.SubFaction)
                .HasForeignKey(x => x.SubFactionId);
        }
    }
}
