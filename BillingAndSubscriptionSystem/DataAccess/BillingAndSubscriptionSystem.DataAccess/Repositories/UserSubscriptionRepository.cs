using BillingAndSubscriptionSystem.Context;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillingAndSubscriptionSystem.DataAccess.Repositories
{
    public class UserSubscriptionRepository : IUserSubscriptionRepository
    {
        private readonly BillingDbContext _context;

        public UserSubscriptionRepository(BillingDbContext context)
        {
            _context = context;
        }

        public async Task CreateUserSubscriptionAsync(Subscription userSubscription)
        {
            await _context.Subscriptions.AddAsync(userSubscription);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubscriptionAsync(Subscription userSubscription)
        {
            _context.Subscriptions.Update(userSubscription);
            await _context.SaveChangesAsync();
        }

        public async Task<Subscription> GetUserSubscriptionAsync(int userId)
        {
            return await _context.Subscriptions.FirstAsync(s => s.UserId == userId)!;
        }

        public async Task<ICollection<Subscription>> GetAllUserSubscriptionsAsync()
        {
            return await _context.Subscriptions.ToListAsync();
        }
    }
}
