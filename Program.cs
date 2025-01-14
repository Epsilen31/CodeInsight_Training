using Codeinsight.FileManager.Services;
using Codeinsight.FileManager.Contracts;

namespace Codeinsight.FileManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IFileProcessor fileProcessor = new FileProcessor();
            IFileServices fileServices = new FileServices(fileProcessor);
            fileServices.FileImplimentationTask();
        }
    }
}
