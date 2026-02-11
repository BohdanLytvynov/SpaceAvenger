using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceAvenger.DAL.Models;

namespace SpaceAvenger.DAL.Configurations.Armors
{
    public class ArmorConfiguration : IEntityTypeConfiguration<Armor>
    {
        public void Configure(EntityTypeBuilder<Armor> builder)
        {
            builder
                .HasMany(x => x.SpaceShipClasses)
                .WithOne(x => x.Armor)
                .HasForeignKey(x => x.ArmorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
