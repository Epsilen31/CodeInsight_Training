namespace Codeinsight.StreamingManagementSystem.BusinessLogic.Contracts
{
    public interface IPaymentService
    {
        void ProcessPayment(PaymentDto payment);
        ICollection<PaymentDto> GetOverduePayments();
    }
}
