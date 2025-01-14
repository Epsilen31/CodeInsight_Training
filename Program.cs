using Codeinsight.FileManager.Services;
using Codeinsight.FileManager.Contracts;

namespace Codeinsight.FileManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IFileServices fileServices = new FileServices();
            fileServices.FileImplimentatiionTask();
        }
    }
}