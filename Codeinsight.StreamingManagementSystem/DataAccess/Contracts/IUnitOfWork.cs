namespace Codeinsight.StreamingManagementSystem.DataAccess.Contracts
{
    public interface IUnitOfWork
    {
        IRepository<Subscription> ManageSubscriptions { get; set; }
        IRepository<Payment> ProcessPayments { get; set; }
        IRepository<Billing> ManageBilling { get; set; }
    }
}
