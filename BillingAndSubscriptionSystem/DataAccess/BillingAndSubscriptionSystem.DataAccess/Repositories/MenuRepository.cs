using BillingAndSubscriptionSystem.Context;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillingAndSubscriptionSystem.DataAccess.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly BillingDbContext _context;

        public MenuRepository(BillingDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Menu>> GetSidebarMenuAsync(
            string role,
            CancellationToken cancellationToken
        )
        {
            return await _context
                .Menus.Include(menu => menu.SubMenus)
                .Where(menu => menu.IsActive && menu.Role == role)
                .ToListAsync(cancellationToken);
        }
    }
}
