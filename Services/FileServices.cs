using Codeinsight.FileManager.Contracts;
using Codeinsight.FileManager.Services;

namespace Codeinsight.FileManager.Services
{
   internal class FileServices : IFileServices
    {
        private IFileProcessor fileProcess { get; set; }
        // Constructor accepting IFileProcessor
        public FileServices(IFileProcessor fileProcessor)
        {
            fileProcess = fileProcessor;
        }

        public void FileImplimentationTask()
        {
            try
            {
                string  filePath = FilePath.filePathValue;
                string beforeMoveddestinationPath = FilePath.beforeMovedDestinationPathValue;
                string afterMoveddestinationPath = FilePath.afterMovedSourcePathValue;

                // Read file content
                string readContent = fileProcess.ReadFile(filePath);
                Console.WriteLine("File Content:");
                Console.WriteLine(readContent);

                // Write content to the file (uncomment one method as needed)
                // Console.WriteLine("Enter something to write into the file:");
                // string? userInput = Console.ReadLine();
                // fileProcess.WriteFile(FilePath, userInput ?? string.Empty);

                // Method 2
                // fileProcess.WriteFile(FilePath, "Hello Abhishek twice, 332");

                // Copy file to another location
                // fileProcess.CopyFile(FilePath, beforeMoveddestinationPath);
                // Console.WriteLine($"File copied to {beforeMoveddestinationPath}");

                // Delete a file (if required)
                // fileProcess.DeleteFile(beforeMoveddestinationPath);
                // Console.WriteLine($"File deleted at {beforeMoveddestinationPath}");

                // Move file to a new location
                // fileProcess.MoveFile(BeforeMovedDestinationPath, afterMoveddestinationPath);
                // Console.WriteLine($"File moved to {afterMoveddestinationPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}

