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

        public async Task CreateUserSubscriptionAsync(
            Subscription userSubscription,
            CancellationToken cancellationToken
        )
        {
            await _context.Subscriptions.AddAsync(userSubscription, cancellationToken);
        }

        public async Task UpdateSubscriptionAsync(
            Subscription userSubscription,
            CancellationToken cancellationToken
        )
        {
            _context.Subscriptions.Update(userSubscription);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Subscription> GetUserSubscriptionAsync(
            int userId,
            CancellationToken cancellationToken
        )
        {
            return await _context.Subscriptions.FirstAsync(
                s => s.UserId == userId,
                cancellationToken
            )!;
        }

        public async Task<ICollection<Subscription>> GetAllUserSubscriptionsAsync(
            CancellationToken cancellationToken
        )
        {
            return await _context.Subscriptions.ToListAsync(cancellationToken);
        }
    }
}
