using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VitalVoyage.Models.Entities;

namespace VitalVoyage.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //primary key
            builder.HasKey(u => u.Id);

            //email: unique and required
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(u => u.Email)
                .IsUnique();

            //password: required
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
