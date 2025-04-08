using BillingAndSubscriptionSystem.Context;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.DataAccess.Models;
using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Entities.Enums;
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
            await _context.Subscriptions.ExecuteUpdateAsync(
                s =>
                    s.SetProperty(u => u.PlanType, userSubscription.PlanType)
                        .SetProperty(u => u.SubscriptionStatus, userSubscription.SubscriptionStatus)
                        .SetProperty(u => u.StartDate, userSubscription.StartDate)
                        .SetProperty(u => u.EndDate, userSubscription.EndDate),
                cancellationToken
            );
        }

        public async Task<Subscription?> GetUserSubscriptionAsync(
            int userId,
            CancellationToken cancellationToken
        )
        {
            return await _context
                .Subscriptions.Where(us => us.UserId == userId)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<ICollection<Subscription>> GetAllUserSubscriptionsAsync(
            CancellationToken cancellationToken
        )
        {
            return await _context.Subscriptions.ToListAsync(cancellationToken);
        }

        public async Task<int> GetInactiveSubscriptionsCountAsync(
            CancellationToken cancellationToken
        )
        {
            //  checking for inactive subscriptions
            return await _context
                .Subscriptions.Where(subscription =>
                    subscription.SubscriptionStatus == SubscriptionStatus.Inactive
                )
                .CountAsync(cancellationToken);
        }

        public async Task<ICollection<MonthlySubscriptionData>> GetMonthlySubscriptionsAsync(
            CancellationToken cancellationToken
        )
        {
            var data = await _context
                .Subscriptions.GroupBy(subscription => new
                {
                    subscription.StartDate.Year,
                    subscription.StartDate.Month,
                })
                .Select(group => new MonthlySubscriptionData
                {
                    Month = $"{group.Key.Year}-{group.Key.Month:D2}",
                    Count = group.Count(),
                })
                .ToListAsync(cancellationToken);

            return data;
        }

        public async Task<ICollection<PlanTypeCountData>> GetSubscriptionPlanCountsAsync(
            CancellationToken cancellationToken
        )
        {
            var data = await _context
                .Subscriptions.GroupBy(subscription => subscription.PlanType)
                .Select(group => new PlanTypeCountData
                {
                    PlanType = group.Key.ToString(),
                    Count = group.Count(),
                })
                .ToListAsync(cancellationToken);

            return data;
        }

        public async Task<bool> DeleteUserSubscriptionAsync(
            int subscriptionId,
            CancellationToken cancellationToken
        )
        {
            var subscription = await _context.Subscriptions.FindAsync(
                [subscriptionId],
                cancellationToken
            );
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
                return true;
            }
            return false;
        }
    }
}
