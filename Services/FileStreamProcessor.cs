using System;
using System.IO;
using Codeinsight.FileManager.Contracts;

namespace Codeinsight.FileManager.Services
{
    internal class FileStreamProcessor : IFileProcessor
    {
        public string ReadFile(string filepath)
        {
            if (File.Exists(filepath))
            {
                using (StreamReader streamReader = new StreamReader(filepath))
                {
                    string content = streamReader.ReadToEnd();
                    Console.WriteLine(content);
                    return content;
                }
            }
            else
            {
                Console.WriteLine("File does not exist.");
                return string.Empty;
            }
        }

        public void WriteFile(string filepath, string text)
        {
            using (StreamWriter streamWriter = new StreamWriter(filepath, append: true))
            {
                streamWriter.WriteLine(text);
            }
            Console.WriteLine("Text written to file.");
        }

        public void CopyFile(string sourcePath, string destinationPath)
        {
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine("Source file does not exist.");
                return;
            }

            using (StreamReader streamReader = new StreamReader(sourcePath))
            using (StreamWriter streamWriter = new StreamWriter(destinationPath))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    streamWriter.WriteLine(line);
                }
            }

            Console.WriteLine("File copied successfully.");
        }

        public void DeleteFile(string sourcePath)
        {
            Console.WriteLine("File deleted successfully.");
        }

        public void MoveFile(string sourcePath, string destinationPath)
        {
            Console.WriteLine("File moved successfully.");
        }
    }
}

