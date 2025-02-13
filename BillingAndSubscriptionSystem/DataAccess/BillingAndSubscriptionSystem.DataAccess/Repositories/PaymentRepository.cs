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

        public async Task ProcessPaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
        }

        public async Task<ICollection<Payment>> OverduePayments()
        {
            return await _context
                .Payments.Where(payment => payment.PaymentStatus == PaymentStatus.Overdue)
                .ToListAsync();
        }
    }
}
