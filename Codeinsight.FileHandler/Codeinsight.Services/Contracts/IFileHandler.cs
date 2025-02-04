namespace Codeinsight.Services.Contracts
{
    public interface IFileHandler
    {
        public Task<string> ReadFile(string filePath);

        public Task WriteToFile(string filePath, string content);

        public Task CopyFile(string sourcePath, string destinationPath);

        public Task DeleteFile(string path);
    }
}
