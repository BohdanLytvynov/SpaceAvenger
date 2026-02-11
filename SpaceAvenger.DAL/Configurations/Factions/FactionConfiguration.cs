using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceAvenger.DAL.Models;

namespace SpaceAvenger.DAL.Configurations.Factions
{
    public class FactionConfiguration : IEntityTypeConfiguration<Faction>
    {
        public void Configure(EntityTypeBuilder<Faction> builder)
        {
            builder
                .HasMany(x => x.SpaceShips)
                .WithOne(x => x.Faction)
                .HasForeignKey(x => x.FactionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(x => x.SpaceShipClasses)
                .WithOne(x => x.Faction)
                .HasForeignKey(x => x.FactionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(x => x.Planets)
                .WithOne(x => x.Faction)
                .HasForeignKey(x => x.FactionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(x => x.SubFactions)
                .WithOne(x => x.Faction)
                .HasForeignKey(x => x.FactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.Weapons)
                .WithOne(x => x.Faction)
                .HasForeignKey(x => x.FactionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(x => x.Projectiles)
                .WithOne(x => x.Faction)
                .HasForeignKey(x => x.FactionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(x => x.Ranks)
                .WithOne(x => x.Faction)
                .HasForeignKey(x => x.FactionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(x => x.Armors)
                .WithOne(x => x.Faction)
                .HasForeignKey(x => x.FactionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(x => x.CommanderRanks)
                .WithOne(x => x.Faction)
                .HasForeignKey(x => x.FactionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
