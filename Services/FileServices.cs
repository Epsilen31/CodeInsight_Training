using Codeinsight.FileManager.Contracts;
using System;

namespace Codeinsight.FileManager.Services
{
    internal class FileServices : IFileServices
    {
        private IFileProcessor FileProcess { get; set; }

        // Constructor accepting IFileProcessor
        public FileServices(IFileProcessor fileProcessor)
        {
            FileProcess = fileProcessor;
        }

        public void PerformFileOperations()
        {
            try
            {
                string filePath = FilePath.filePathValue;

                ReadFile(filePath);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred: {exception.Message}");
            }
        }

        public void ReadFile(string filePath)
        {
            string content = FileProcess.ReadFile(filePath);
            Console.WriteLine(content);
        }

        public void WriteToFile(string filePath, string content)
        {
            FileProcess.WriteFile(filePath, content);
            Console.WriteLine("Content written to file.");
        }

        public  void CopyFile(string sourcePath, string destinationPath)
        {
            FileProcess.CopyFile(sourcePath, destinationPath);
            Console.WriteLine("File copied to Sucessfully");
        }

        public void DeleteFile(string filePath)
        {
            FileProcess.DeleteFile(filePath);
            Console.WriteLine("File deleted Sucessfully");
        }

        public void MoveFile(string sourcePath, string destinationPath)
        {
            FileProcess.MoveFile(sourcePath, destinationPath);
            Console.WriteLine("File moved Sucessfully");
        }
    }
}

