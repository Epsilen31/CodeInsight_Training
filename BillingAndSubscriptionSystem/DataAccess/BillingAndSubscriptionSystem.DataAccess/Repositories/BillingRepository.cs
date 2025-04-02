using BillingAndSubscriptionSystem.Context;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillingAndSubscriptionSystem.DataAccess.Repositories
{
    public class BillingRepository : IBillingRepository
    {
        private readonly BillingDbContext _context;

        public BillingRepository(BillingDbContext context)
        {
            _context = context;
        }

        public async Task UpdateBillingAsync(
            Billing billingDetails,
            CancellationToken cancellationToken
        )
        {
            await _context
                .Billings.Where(x => x.UserId == billingDetails.UserId)
                .ExecuteUpdateAsync(
                    ex =>
                        ex.SetProperty(x => x.PaymentMethod, billingDetails.PaymentMethod)
                            .SetProperty(x => x.BillingAddress, billingDetails.BillingAddress)
                            .SetProperty(x => x.UserId, billingDetails.UserId),
                    cancellationToken
                );
        }

        public async Task<ICollection<Billing>> GetAllUsersWithBillingDetails(
            int userId,
            CancellationToken cancellationToken
        )
        {
            return await _context
                .Billings.Where(billing => billing.UserId == userId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        // Remove billing details
        public async Task RemoveBillingDetailsAsync(int userId, CancellationToken cancellationToken)
        {
            var billing = await _context.Billings.FindAsync([userId], cancellationToken);
            if (billing != null)
            {
                _context.Billings.Remove(billing);
            }
        }
    }
}
