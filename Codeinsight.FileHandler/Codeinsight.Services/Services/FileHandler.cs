using Codeinsight.Services.Contracts;

namespace Codeinsight.Services.Services
{
    public class FileHandler : IFileHandler
    {
        public async Task<string> ReadFile(string filePath)
        {
            try
            {
                var tokenSource = new CancellationTokenSource();
                var lines = await File.ReadAllLinesAsync(filePath, tokenSource.Token);
                Console.WriteLine("CSV file is read successfully");
                return string.Join(Environment.NewLine, lines);
            }
            catch
            {
                throw;
            }
        }

        public async Task WriteToFile(string filePath, string content)
        {
            try
            {
                var tokenSource = new CancellationTokenSource();
                await File.WriteAllTextAsync(filePath, content, tokenSource.Token);
            }
            catch
            {
                throw;
            }
        }

        public async Task CopyFile(string sourcePath, string destinationPath)
        {
            try
            {
                var tokenSource = new CancellationTokenSource();
                await Task.Run(
                    () => File.Copy(sourcePath, destinationPath, true),
                    tokenSource.Token
                );
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteFile(string path)
        {
            try
            {
                var tokenSource = new CancellationTokenSource();
                await Task.Run(() => File.Delete(path), tokenSource.Token);
            }
            catch
            {
                throw;
            }
        }
    }
}
