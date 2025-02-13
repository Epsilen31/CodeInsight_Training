using BillingAndSubscriptionSystem.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingAndSubscriptionSystem.Context.Configurations
{
    public class BillingConfiguration : IEntityTypeConfiguration<Billing>
    {
        public void Configure(EntityTypeBuilder<Billing> builder)
        {
            builder.ToTable("Billings");
            builder.HasKey(billing => billing.Id);
            builder.Property(billing => billing.Id).ValueGeneratedOnAdd();
            builder.Property(billing => billing.PaymentMethod).IsRequired();
            builder.Property(billing => billing.BillingAddress).HasMaxLength(100);
            builder
                .HasOne(billing => billing.User)
                .WithMany(billing => billing.Billings)
                .HasForeignKey(billing => billing.UserId);
        }
    }
}
