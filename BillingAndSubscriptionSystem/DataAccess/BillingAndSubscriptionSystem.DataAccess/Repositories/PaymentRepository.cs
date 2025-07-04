using BillingAndSubscriptionSystem.Context;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Entities.Entities;
using BillingAndSubscriptionSystem.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace BillingAndSubscriptionSystem.DataAccess.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly BillingDbContext _context;

        public PaymentRepository(BillingDbContext context)
        {
            _context = context;
        }

        public async Task ProcessPaymentAsync(Payment payment, CancellationToken cancellationToken)
        {
            await _context.Payments.AddAsync(payment, cancellationToken);
        }

        public async Task<ICollection<Payment>> OverduePaymentsAsync(
            CancellationToken cancellationToken
        )
        {
            return await _context
                .Payments.Where(payment => payment.PaymentStatus == PaymentStatus.Overdue)
                .ToListAsync(cancellationToken);
        }

        // counting overdue payments
        public async Task<int> GetOverduePaymentsCountAsync(CancellationToken cancellationToken)
        {
            return (await OverduePaymentsAsync(cancellationToken)).Count;
        }

        // counting total payments
        public async Task<int> GetTotalPaymentsCountAsync(CancellationToken cancellationToken)
        {
            return await _context.Payments.CountAsync(cancellationToken);
        }
    }
}
