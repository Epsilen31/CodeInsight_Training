using BillingAndSubscriptionSystem.Context;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillingAndSubscriptionSystem.DataAccess.Repositories
{
    public class SubMenuRepository : ISubMenuRepository
    {
        private readonly BillingDbContext _context;

        public SubMenuRepository(BillingDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<SubMenu>> GetAllActiveSubMenusAsync(
            CancellationToken cancellationToken
        )
        {
            return await _context
                .SubMenus.Where(subMenu => subMenu.IsActive)
                .ToListAsync(cancellationToken);
        }
    }
}
