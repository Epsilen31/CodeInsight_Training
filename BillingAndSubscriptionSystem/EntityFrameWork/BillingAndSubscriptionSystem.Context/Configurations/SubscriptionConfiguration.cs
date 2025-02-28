using BillingAndSubscriptionSystem.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingAndSubscriptionSystem.Context.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("Subscriptions");
            builder.HasKey(subscription => subscription.Id);
            builder.Property(subscription => subscription.Id).ValueGeneratedOnAdd();

            builder.Property(subscription => subscription.PlanType).IsRequired();
            builder.Property(subscription => subscription.StartDate).IsRequired();
            builder.Property(subscription => subscription.EndDate).IsRequired();
            builder.Property(subscription => subscription.SubscriptionStatus).IsRequired();
            builder
                .HasOne(subscription => subscription.User)
                .WithMany(user => user.Subscriptions)
                .HasForeignKey(subscription => subscription.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
