using BillingAndSubscriptionSystem.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingAndSubscriptionSystem.Context.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(user => user.Id);
            builder.Property(user => user.Id).ValueGeneratedOnAdd();
            builder.Property(user => user.Name).IsRequired().HasMaxLength(50);
            builder.Property(user => user.Email).HasMaxLength(50);
            builder.HasIndex(user => user.Email).IsUnique();
            builder.Property(user => user.Phone).HasMaxLength(15);
        }
    }
}
