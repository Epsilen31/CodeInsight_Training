using Codeinsight.Services.Contracts;
using Codeinsight.WebApi.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Codeinsight.WebApi.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> ReadFile()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_filePaths.BaseFile))
                    return BadRequest("File path cannot be empty.");

                string content = await _fileHandler.ReadFile(Path.GetFullPath(_filePaths.BaseFile));
                return Ok(content);
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Error reading file: {exception.Message}");
            }
        }

        [HttpPost(RouteKey.WriteRoute)]
        public async Task<IActionResult> WriteFile(string content)
        {
            try
            {
                if (
                    string.IsNullOrWhiteSpace(_filePaths.BaseFile)
                    || string.IsNullOrWhiteSpace(content)
                )
                    return BadRequest("File path and content cannot be empty.");

                await _fileHandler.WriteToFile(_filePaths.BaseFile, content);
                return Ok("File written successfully.");
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Error writing file");
                return StatusCode(500, $"Error writing to file: {exception.Message}");
            }
        }

        [HttpDelete(RouteKey.DeleteRoute)]
        public async Task<IActionResult> DeleteFile()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_filePaths.CopyFile))
                    return BadRequest("File path cannot be empty.");

                await _fileHandler.DeleteFile(_filePaths.CopyFile);
                return Ok("File deleted successfully.");
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine($"Error while deleting file");
                return StatusCode(500, $"Error deleting file: {exception.Message}");
            }
        }

        [HttpPost(RouteKey.CopyRoute)]
        public async Task<IActionResult> CopyFile()
        {
            try
            {
                if (
                    string.IsNullOrWhiteSpace(_filePaths.BaseFile)
                    || string.IsNullOrWhiteSpace(_filePaths.CopyFile)
                )
                    return BadRequest("Source and destination paths cannot be empty.");

                await _fileHandler.CopyFile(
                    Path.GetFullPath(_filePaths.BaseFile),
                    Path.GetFullPath(_filePaths.CopyFile)
                );
                return Ok("File copied successfully.");
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine($"Error while copying file");
                return StatusCode(500, $"Error copying file: {exception.Message}");
            }
        }
    }
}
