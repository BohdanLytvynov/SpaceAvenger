using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceAvenger.DAL.Models;

namespace SpaceAvenger.DAL.Configurations.SpaceShips
{
    public class SpaceShipConfiguration : IEntityTypeConfiguration<SpaceShip>
    {
        public void Configure(EntityTypeBuilder<SpaceShip> builder)
        {
            builder
                .HasOne(x => x.SpaceShipClass)
                .WithMany(x => x.SpaceShips)
                .HasForeignKey(x => x.SpaceShipClassId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
