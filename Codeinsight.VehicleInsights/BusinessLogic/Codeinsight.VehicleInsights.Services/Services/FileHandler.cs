using Codeinsight.VehicleInsights.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace Codeinsight.VehicleInsights.Services.Services
{
    public class FileHandler : IFileHandler
    {
        private readonly ILogger<FileHandler> _logger;

        public FileHandler(ILogger<FileHandler> logger)
        {
            _logger = logger;
        }

        public async Task<string> ReadFileAsync(
            string filePath,
            CancellationToken cancellationToken
        )
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"File {filePath} not found.");
                }
                var lines = await File.ReadAllLinesAsync(
                    Path.GetFullPath(filePath),
                    cancellationToken
                );
                return string.Join(Environment.NewLine, lines);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error {Filepath} reading file", exception.Message);
                throw new InvalidOperationException("Cannot read file " + filePath);
            }
        }

        public async Task GenerateFileAsync(
            string filePath,
            string content,
            CancellationToken cancellationToken
        )
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"File {filePath} not found.");
                }
                await File.WriteAllTextAsync(
                    Path.GetFullPath(filePath),
                    content,
                    cancellationToken
                );
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Error {FiePath} generating file : {Exception} ",
                    filePath,
                    exception.Message
                );
                throw new FileNotFoundException($"File {filePath} not found.");
            }
        }
    }
}
