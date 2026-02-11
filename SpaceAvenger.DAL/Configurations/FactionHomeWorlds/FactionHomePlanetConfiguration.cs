using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceAvenger.DAL.Models;
using System.Reflection.Emit;

namespace SpaceAvenger.DAL.Configurations.FactionHomeWorlds
{
    public class FactionHomeWorldConfiguration : IEntityTypeConfiguration<FactionHomeWorld>
    {
        public void Configure(EntityTypeBuilder<FactionHomeWorld> builder)
        {
            builder
                .HasKey(x => new { x.FactionId, x.HomePlanetId });

            builder
                .HasOne(x => x.Faction)
                .WithMany(x => x.FactionHomeWorlds)
                .HasForeignKey(x => x.FactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.HomePlanet)
                .WithMany(x => x.FactionHomePlanets)
                .HasForeignKey(x => x.HomePlanetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
