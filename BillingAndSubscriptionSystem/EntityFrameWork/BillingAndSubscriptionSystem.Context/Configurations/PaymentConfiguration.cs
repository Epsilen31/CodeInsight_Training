using BillingAndSubscriptionSystem.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingAndSubscriptionSystem.Context.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(payment => payment.Id);
            builder.Property(payment => payment.Id).ValueGeneratedOnAdd();

            builder.Property(payment => payment.Amount).IsRequired();
            builder.Property(payment => payment.PaymentDate).IsRequired();
            builder.Property(payment => payment.PaymentStatus).IsRequired();
            builder
                .HasOne(payment => payment.Subscription)
                .WithMany(subscription => subscription.Payments)
                .HasForeignKey(payment => payment.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
