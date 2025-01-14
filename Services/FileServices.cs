using Codeinsight.FileManager.Contracts;

namespace Codeinsight.FileManager.Services
{
    internal class FileServices : IFileServices
    {
        IFileProcessor fileProcess = new FileProcessor();

        //private const string filePath = "E:\\C#\\FileHandler\\Codeinsight.FileManager\\Codeinsight.FileManager\\Contracts\\Text.csv";
        //private const string beforeMoveddestinationPath = "E:\\C#\\FileHandler\\FileHandler\\Services\\copyText.csv";
        //private const string afterMovedsourcePath = "E:\\C#\\FileHandler\\FileHandler\\Contracts\\copyText.csv";


        //public string FilePath { get => filePath; }
        //public string BeforeMoveddestinationPath { get => beforeMoveddestinationPath; }
        //public string AfterMovedsourcePath { get => afterMovedsourcePath; }


        public void FileImplimentatiionTask()
        {
            const string filePath = "E:\\C#\\FileHandler\\Codeinsight.FileManager\\Codeinsight.FileManager\\Contracts\\Text.csv";
            //const string beforeMoveddestinationPath = "E:\\C#\\FileHandler\\FileHandler\\Services\\copyText.csv";
            //const string afterMovedsourcePath = "E:\\C#\\FileHandler\\FileHandler\\Contracts\\copyText.csv";


            string ReadContent = fileProcess.ReadFile(filePath);
            Console.WriteLine(ReadContent);


            //Method 1
            // Console.WriteLine("Enter Something Here to Add...");
            //string? userInput = Console.ReadLine();
            //string writContent = fileProcess.WriteFile(FilePath, userInput ?? string.Empty);

            //Method 2
            //fileProcess.WriteFile(FilePath, "Hello Abhishek twice, 332");

            //fileProcess.DeleteFile(BeforeMoveddestinationPath);

            //fileProcess.CopyFile(FilePath, BeforeMoveddestinationPath);

            //fileProcess.MoveFile(BeforeMoveddestinationPath, AfterMovedsourcePath);

        }

    }
}
