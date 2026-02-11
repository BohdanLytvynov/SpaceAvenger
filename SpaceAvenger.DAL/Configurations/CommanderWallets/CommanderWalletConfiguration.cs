using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceAvenger.DAL.Models;

namespace SpaceAvenger.DAL.Configurations.CommanderWallets
{
    public class CommanderWalletConfiguration : IEntityTypeConfiguration<CommanderWallet>
    {
        public void Configure(EntityTypeBuilder<CommanderWallet> builder)
        {
            builder
                .HasOne(x => x.Currency)
                .WithMany(x => x.CommanderWallets)
                .HasForeignKey(x => x.CurrencyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.Commander)
                .WithMany(x => x.CommanderWallets)
                .HasForeignKey(x => x.CommanderId)
                .OnDelete(DeleteBehavior.Cascade);//Delete Commander - Delete Wallets

            builder
                .Property(x => x.Amount)
                .HasPrecision(18, 2);

            builder
                .HasIndex(x => new { x.CommanderId, x.CurrencyId })
                .IsUnique();
        }
    }
}
