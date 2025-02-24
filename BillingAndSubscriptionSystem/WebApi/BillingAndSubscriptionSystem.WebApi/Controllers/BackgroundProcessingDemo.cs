using BillingAndSubscriptionSystem.Core.BackGround;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingAndSubscriptionSystem.WebApi.Controllers
{
    [Authorize]
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : BaseController
    {
        private readonly BackGroundQueue _taskQueue;

        public TaskController(BackGroundQueue queue)
        {
            _taskQueue = queue;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessTask()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                string taskName = await reader.ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(taskName))
                {
                    return BadRequest(new { Message = "TaskName is required." });
                }

                _taskQueue.QueueTask(
                    async (serviceProvider, cancellationToken) =>
                    {
                        await Task.Delay(2000, cancellationToken);
                        Console.WriteLine($"Task Processed: {taskName}");
                    }
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return Accepted(new { Message = "Task is being processed in the background." });
        }
    }
}
