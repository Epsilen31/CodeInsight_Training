namespace BillingAndSubscriptionSystem.DataAccess.Contracts
{
    public interface IUnitOfWork
    {
        IUserSubscriptionRepository UserSubscriptionRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IBillingRepository BillingRepository { get; }
        IUserRepository UserRepository { get; }
    }
}
