using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceAvenger.DAL.Models;

namespace SpaceAvenger.DAL.Configurations.BonusParameters
{
    public class BonusParameterConfiguration : IEntityTypeConfiguration<BonusParameter>
    {
        public void Configure(EntityTypeBuilder<BonusParameter> builder)
        {
            builder.Property(x => x.ParameterName).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.ParameterName).IsUnique();
        }
    }
}
