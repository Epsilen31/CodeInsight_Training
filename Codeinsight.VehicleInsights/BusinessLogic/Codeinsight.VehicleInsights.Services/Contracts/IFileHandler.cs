namespace Codeinsight.VehicleInsights.Services.Contracts
{
    public interface IFileHandler
    {
        public Task<string> ReadFileAsync(string filePath, CancellationToken token);

        public Task GenerateFileAsync(string filePath, string content, CancellationToken token);
    }
}
