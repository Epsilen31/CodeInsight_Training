using BillingAndSubscriptionSystem.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingAndSubscriptionSystem.Context.Configurations
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("Menus");

            builder.HasKey(menu => menu.Id);

            builder.Property(menu => menu.Title).HasMaxLength(100).IsRequired();
            builder.Property(menu => menu.Path).HasMaxLength(255);
            builder.Property(menu => menu.Role).HasMaxLength(50).IsRequired();
            builder.Property(menu => menu.Icon).HasMaxLength(100);

            // One Menu can have multiple SubMenus
            builder
                .HasMany(menu => menu.SubMenus)
                .WithOne(subMenu => subMenu.Menu)
                .HasForeignKey(subMenu => subMenu.MenuId)
                .OnDelete(DeleteBehavior.Cascade); // menu deleted submenu also deleted
        }
    }
}
