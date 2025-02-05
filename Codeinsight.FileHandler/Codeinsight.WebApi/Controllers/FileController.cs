using Codeinsight.Core.Settings;
using Codeinsight.Services.Contracts;
using Codeinsight.WebApi.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Codeinsight.WebApi.Controllers
{
    [Route(RouteKey.HeadRoute)]
    [ApiController]
    public class FileController : BaseController
    {
        private readonly IFileHandler _fileHandler;
        private readonly FilePaths _filePaths;

        public FileController(IFileHandler fileHandler, IOptions<FilePaths> filePaths)
        {
            _fileHandler = fileHandler;
            _filePaths = filePaths.Value;
        }

        [HttpGet(RouteKey.ReadRoute)]
        public async Task<IActionResult> ReadFile(CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_filePaths.BaseFile))
                    return BadRequest("File path cannot be empty.");

                string content = await _fileHandler.ReadFileAsync(
                    Path.GetFullPath(_filePaths.BaseFile),
                    cancellationToken
                );
                return Ok(content);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error reading file: {exception.Message}");
                return StatusCode(500, $"Error reading file: {exception.Message}");
            }
        }

        [HttpPost(RouteKey.WriteRoute)]
        public async Task<IActionResult> WriteFile(
            [FromBody] string content,
            CancellationToken cancellationToken
        )
        {
            try
            {
                if (
                    string.IsNullOrWhiteSpace(_filePaths.BaseFile)
                    || string.IsNullOrWhiteSpace(content)
                )
                    return BadRequest("File path and content cannot be empty.");

                await _fileHandler.WriteToFileAsync(
                    Path.GetFullPath(_filePaths.BaseFile),
                    content,
                    cancellationToken
                );
                return Ok("File written successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error writing to file: {exception.Message}");
                return StatusCode(500, $"Error writing to file: {exception.Message}");
            }
        }

        [HttpDelete(RouteKey.DeleteRoute)]
        public async Task<IActionResult> DeleteFile(CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_filePaths.CopyFile))
                    return BadRequest("File path cannot be empty.");

                await _fileHandler.DeleteFileAsync(
                    Path.GetFullPath(_filePaths.CopyFile),
                    cancellationToken
                );
                return Ok("File deleted successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error deleting file: {exception.Message}");
                return StatusCode(500, $"Error deleting file: {exception.Message}");
            }
        }

        [HttpPost(RouteKey.CopyRoute)]
        public async Task<IActionResult> CopyFile(CancellationToken cancellationToken)
        {
            try
            {
                if (
                    string.IsNullOrWhiteSpace(_filePaths.BaseFile)
                    || string.IsNullOrWhiteSpace(_filePaths.CopyFile)
                )
                    return BadRequest("Source and destination paths cannot be empty.");

                await _fileHandler.CopyFileAsync(
                    Path.GetFullPath(_filePaths.BaseFile),
                    Path.GetFullPath(_filePaths.CopyFile),
                    cancellationToken
                );
                return Ok("File copied successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error copying file: {exception.Message}");
                return StatusCode(500, $"Error copying file: {exception.Message}");
            }
        }
    }
}
