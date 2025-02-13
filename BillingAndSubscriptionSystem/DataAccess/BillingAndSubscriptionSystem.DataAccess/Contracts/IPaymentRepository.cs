using BillingAndSubscriptionSystem.Entities.Entities;

namespace BillingAndSubscriptionSystem.DataAccess.Contracts
{
    public interface IPaymentRepository
    {
        Task ProcessPaymentAsync(Payment payment);
        Task<ICollection<Payment>> OverduePayments();
    }
}
