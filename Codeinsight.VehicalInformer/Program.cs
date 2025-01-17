using Codeinsight.VehicalInformer.Contracts;
using Codeinsight.VehicalInformer.Services;

namespace Codeinsight.VehicalInformer
{
    public class Program
    {
        static void Main(string[] args)
        {
            IFileProcessor fileProcessor = new FileProcessor();
            IVehicalService vehicalService = new CarServices(fileProcessor);
            vehicalService.GenerateCarReport();
            vehicalService.ConsoleCarReport();
        }
    }
}

