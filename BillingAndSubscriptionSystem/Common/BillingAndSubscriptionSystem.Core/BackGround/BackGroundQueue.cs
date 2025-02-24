using System.Collections.Concurrent;

namespace BillingAndSubscriptionSystem.Core.BackGround
{
    public class BackGroundQueue
    {
        private static readonly ConcurrentQueue<
            Func<IServiceProvider, CancellationToken, Task>
        > _queue = new();
        private static readonly SemaphoreSlim _signal = new(0);

        public void QueueTask(Func<IServiceProvider, CancellationToken, Task> workItem)
        {
            if (workItem == null)
                throw new ArgumentNullException(nameof(workItem));

            _queue.Enqueue(workItem);
            _signal.Release();
        }

        public async Task<Func<IServiceProvider, CancellationToken, Task>> DequeueAsync(
            CancellationToken cancellationToken
        )
        {
            await _signal.WaitAsync(cancellationToken);
            _queue.TryDequeue(out var workItem);
            return workItem!;
        }
    }
}
