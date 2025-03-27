using BillingAndSubscriptionSystem.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingAndSubscriptionSystem.Context.Configurations
{
    public class SubMenuConfiguration : IEntityTypeConfiguration<SubMenu>
    {
        public void Configure(EntityTypeBuilder<SubMenu> builder)
        {
            builder.ToTable("SubMenus");

            builder.HasKey(subMenu => subMenu.Id);

            builder.Property(subMenu => subMenu.Title).HasMaxLength(100).IsRequired();
            builder.Property(subMenu => subMenu.Path).HasMaxLength(255);
            builder.Property(submenu => submenu.Icon).HasMaxLength(100);

            builder
                .HasOne(subMenu => subMenu.Menu)
                .WithMany(menu => menu.SubMenus)
                .HasForeignKey(subMenu => subMenu.MenuId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
