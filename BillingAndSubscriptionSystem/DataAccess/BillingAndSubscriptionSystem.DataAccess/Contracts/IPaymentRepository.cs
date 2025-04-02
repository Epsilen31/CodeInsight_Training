using BillingAndSubscriptionSystem.Entities.Entities;

namespace BillingAndSubscriptionSystem.DataAccess.Contracts
{
    public interface IPaymentRepository
    {
        Task ProcessPaymentAsync(Payment payment, CancellationToken cancellationToken);
        Task<ICollection<Payment>> OverduePaymentsAsync(CancellationToken cancellationToken);
        Task<int> GetOverduePaymentsCountAsync(CancellationToken cancellationToken);

        Task<int> GetTotalPaymentsCountAsync(CancellationToken cancellationToken);
    }
}
