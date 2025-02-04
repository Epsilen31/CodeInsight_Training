using Codeinsight.Services.Contracts;

namespace Codeinsight.Services.Services
{
    public class FileHandler : IFileHandler
    {
        public async Task<string> ReadFileAsync(string filePath, CancellationToken token)
        {
            try
            {
                var lines = await File.ReadAllLinesAsync(filePath, token);
                Console.WriteLine("CSV file is read successfully");
                return string.Join(Environment.NewLine, lines);
            }
            catch
            {
                throw;
            }
        }

        public async Task WriteToFileAsync(string filePath, string content, CancellationToken token)
        {
            try
            {
                await File.WriteAllTextAsync(filePath, content, token);
            }
            catch
            {
                throw;
            }
        }

        public async Task CopyFileAsync(
            string sourcePath,
            string destinationPath,
            CancellationToken token
        )
        {
            try
            {
                await Task.Run(() => File.Copy(sourcePath, destinationPath, true), token);
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteFileAsync(string path, CancellationToken token)
        {
            try
            {
                await Task.Run(() => File.Delete(path), token);
            }
            catch
            {
                throw;
            }
        }
    }
}
