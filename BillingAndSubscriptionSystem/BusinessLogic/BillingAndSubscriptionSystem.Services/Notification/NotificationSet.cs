using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.Core.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Notification
{
    public class NotificationSet
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly ILogger<NotificationHub> _logger;

        public NotificationSet(
            IHubContext<NotificationHub> notificationHubContext,
            ILogger<NotificationHub> logger
        )
        {
            _notificationHubContext = notificationHubContext;
            _logger = logger;
        }

        public async Task SendNotification(string message, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new CustomException("UserId cannot be null or empty.", null);
            }

            try
            {
                await _notificationHubContext
                    .Clients.Group(userId)
                    .SendAsync("ReceiveNotification", message);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error sending notification to {userId}: {exception.Message}");
            }
        }

        public async Task BroadcastNotification(string message)
        {
            try
            {
                await _notificationHubContext.Clients.All.SendAsync("ReceiveNotification", message);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error broadcasting notification: {exception.Message}");
            }
        }
    }
}
