using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceAvenger.DAL.Models;

namespace SpaceAvenger.DAL.Configurations.Commanders
{
    internal class CommanderConfiguration : IEntityTypeConfiguration<Commander>
    {
        public void Configure(EntityTypeBuilder<Commander> builder)
        {
            builder
                .HasOne(x => x.Faction)
                .WithMany(x => x.Commanders)
                .HasForeignKey(x => x.FactionId)
                .OnDelete(DeleteBehavior.SetNull);//Weak ref for dictionary - tables

            builder
                .HasOne(x => x.SubFaction)
                .WithMany(x => x.Commanders)
                .HasForeignKey(x => x.SubFactionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(x => x.SpaceShips)
                .WithOne(x => x.Commander)
                .HasForeignKey(x => x.CommanderId)
                .OnDelete(DeleteBehavior.Cascade);//Delete All Commander Ships when we delete commander            
        }
    }
}
