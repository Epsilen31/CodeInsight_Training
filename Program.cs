using Codeinsight.FileManager.Services;
using Codeinsight.FileManager.Contracts;

namespace Codeinsight.FileManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IFileProcessor fileProcessor = new FileProcessor();
            IFileProcessor fileStreamProcessor = new FileStreamProcessor();

            IFileServices fileStreamService = new FileServices(fileStreamProcessor);
            IFileServices fileProcessorService = new FileServices(fileProcessor);

            fileStreamService.PerformFileOperations();
            fileProcessorService.PerformFileOperations();
        }
    }
}

