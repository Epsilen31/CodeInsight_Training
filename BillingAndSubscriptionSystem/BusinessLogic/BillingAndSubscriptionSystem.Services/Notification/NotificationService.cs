using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.Core.Hubs;
using BillingAndSubscriptionSystem.Services.Contracts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly ILogger<NotificationHub> _logger;

        public NotificationService(
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
                _logger.LogError(
                    exception,
                    "Error sending notification to {UserId}: {ErrorMessage}",
                    userId,
                    exception.Message
                );
            }
        }

        public async Task BroadcastNotification(string message)
        {
            try
            {
                await _notificationHubContext.Clients.All.SendAsync(
                    "ReceiveNotification",
                    message,
                    cancellationToken: CancellationToken.None
                );
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Error broadcasting notification: {ErrorMessage}",
                    exception.Message
                );
            }
        }

        public async Task SendProgressData(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new CustomException("UserId cannot be null or empty.", null);
            }

            try
            {
                int currentProgress = 0;
                Random random = new Random();

                while (currentProgress < 100)
                {
                    // Generated a random increment between 1 and 10
                    int increment = random.Next(1, 10);
                    currentProgress = Math.Min(currentProgress + increment, 100);

                    await _notificationHubContext
                        .Clients.Group(userId)
                        .SendAsync("ReceiveProgress", new { Progress = currentProgress });

                    // Random delay between 200ms and 1000ms
                    int delay = random.Next(200, 1000);
                    await Task.Delay(delay);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Error sending progress updates to {UserId}: {ErrorMessage}",
                    userId,
                    exception.Message
                );
            }
        }
    }
}
