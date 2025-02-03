using Codeinsight.StreamingManagementSystem.DataAccess.Entities;

namespace Codeinsight.StreamingManagementSystem.DataAccess.Contracts
{
    public interface IPaymentRepository
    {
        void ProcessPayment(Payment payment);
        ICollection<Payment> GetOverduePayments();
    }
}
