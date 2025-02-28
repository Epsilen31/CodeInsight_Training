namespace BillingAndSubscriptionSystem.Services.Contracts
{
    public interface INotificationService
    {
        Task SendNotification(string message, string userId);
        Task BroadcastNotification(string message);
    }
}
