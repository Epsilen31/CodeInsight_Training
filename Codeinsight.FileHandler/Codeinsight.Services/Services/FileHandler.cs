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
                throw new Exception("Cannot read file " + filePath);
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
                throw new Exception($"Failed to Write file: {filePath}");
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
                throw new Exception($"Failed to copy file: {sourcePath} to {destinationPath}");
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
                throw new Exception($"Failed to delete file: {path}");
            }
        }
    }
}
