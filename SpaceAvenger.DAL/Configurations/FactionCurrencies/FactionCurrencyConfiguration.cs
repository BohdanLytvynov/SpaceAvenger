using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceAvenger.DAL.Models;
using System.Reflection.Emit;

namespace SpaceAvenger.DAL.Configurations.FactionCurrencies
{
    public class FactionCurrencyConfiguration : IEntityTypeConfiguration<FactionCurrency>
    {
        public void Configure(EntityTypeBuilder<FactionCurrency> builder)
        {
            builder
               .HasKey(x => new { x.CurrencyId, x.FactionId });

            builder
                .HasOne(x => x.Currency)
                .WithMany(x => x.FactionCurrency)
                .HasForeignKey(x => x.CurrencyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.Faction)
                .WithMany(x => x.FactionCurrency)
                .HasForeignKey(x => x.FactionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
