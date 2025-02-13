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

        public async Task UpdateBillingDetails(
            Billing billingDetails,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var existingBilling = await _context.Billings.FindAsync(billingDetails.Id);
                if (existingBilling == null)
                {
                    throw new InvalidOperationException("Billing details not found");
                }

                _context.Entry(existingBilling).CurrentValues.SetValues(billingDetails);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<ICollection<Billing>> GetAllUsersWithBillingDetails(
            CancellationToken cancellationToken
        )
        {
            return await _context.Billings.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
