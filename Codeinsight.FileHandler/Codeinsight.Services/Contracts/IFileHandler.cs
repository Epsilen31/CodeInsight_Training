namespace Codeinsight.Services.Contracts
{
    public interface IFileHandler
    {
        public Task<string> ReadFileAsync(string filePath, CancellationToken token);

        public Task WriteToFileAsync(string filePath, string content, CancellationToken token);

        public Task CopyFileAsync(
            string sourcePath,
            string destinationPath,
            CancellationToken token
        );

        public Task DeleteFileAsync(string path, CancellationToken token);
    }
}
