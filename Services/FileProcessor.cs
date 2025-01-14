using Codeinsight.FileManager.Contracts;

namespace Codeinsight.FileManager.Services
{
    internal class FileProcessor : IFileProcessor
    {
        public string ReadFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return "File does not exist";
            }
            return string.Join("\n", File.ReadAllLines(filepath));
        }

        public void WriteFile(string filepath, string text)
        {
            File.AppendAllText(filepath, "\n" + text);
            Console.WriteLine("Text written to file.");
        }

        public void CopyFile(string sourcePath, string destinationPath)
        {
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine("Source file does not exist");
                return;
            }
            File.Copy(sourcePath, destinationPath, overwrite: true);
            Console.WriteLine("File copied successfully");
        }

        public void DeleteFile(string sourcePath)
        {
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine("Source file does not exist");
                return;
            }
            File.Delete(sourcePath);
            Console.WriteLine("File deleted successfully");
        }

        public void MoveFile(string sourcePath, string destinationPath)
        {
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine("Source file does not exist.");
                return;
            }
            File.Move(sourcePath, destinationPath);
            Console.WriteLine($"File moved successfully to {destinationPath}");
        }
    }
}
