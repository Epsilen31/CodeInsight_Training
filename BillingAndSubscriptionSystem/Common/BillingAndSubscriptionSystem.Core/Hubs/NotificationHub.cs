using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Core.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                if (Context.User?.Identity?.IsAuthenticated != true)
                {
                    _logger.LogWarning(
                        $"Unauthorized WebSocket connection attempt: {Context.ConnectionId}"
                    );
                    Context.Abort();
                    return;
                }

                string? userId =
                    Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? Context.User.FindFirst("sub")?.Value
                    ?? Context.User.FindFirst("id")?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                    _logger.LogInformation(
                        $"User {userId} (ConnectionId: {Context.ConnectionId}) connected to NotificationHub."
                    );
                }
                else
                {
                    _logger.LogError(
                        $"User ID is missing for ConnectionId {Context.ConnectionId}. Disconnecting user."
                    );
                    Context.Abort();
                }

                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during WebSocket connection: {Context.ConnectionId}");
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                if (Context.User?.Identity?.IsAuthenticated == true)
                {
                    string? userId =
                        Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? Context.User.FindFirst("sub")?.Value
                        ?? Context.User.FindFirst("id")?.Value;

                    if (!string.IsNullOrEmpty(userId))
                    {
                        await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
                        _logger.LogInformation(
                            $"User {userId} (ConnectionId: {Context.ConnectionId}) disconnected from NotificationHub."
                        );
                    }
                }

                if (exception is not null)
                {
                    _logger.LogError(exception, "Error during SignalR disconnection.");
                }

                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error during user disconnection.");
            }
        }
    }
}
