using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceAvenger.DAL.Models;
using System.Reflection.Emit;

namespace SpaceAvenger.DAL.Configurations.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(x => x.Commander)
                .WithOne(x => x.User)
                .HasForeignKey<Commander>(x => x.UserId)//Dependent
                .OnDelete(DeleteBehavior.Cascade);//Delete User - Delete Commander
        }
    }
}
