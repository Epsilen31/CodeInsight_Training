using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Core.BackGround
{
    public class BackGroundService : BackgroundService
    {
        private readonly BackGroundQueue _taskQueue;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackGroundService> _logger;

        public BackGroundService(
            BackGroundQueue taskQueue,
            IServiceProvider serviceProvider,
            ILogger<BackGroundService> logger
        )
        {
            _taskQueue = taskQueue;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Queued Processor Background Service is starting.");

            while (!cancellationToken.IsCancellationRequested)
            {
                var workItem = await _taskQueue.DequeueAsync(cancellationToken);

                try
                {
                    await workItem(_serviceProvider, cancellationToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        $"Error processing background task {nameof(workItem)}."
                    );
                }
            }

            _logger.LogInformation("Task Processing Service is stopped.");
        }
    }
}
