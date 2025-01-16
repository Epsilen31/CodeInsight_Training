using Codeinsight.VehicalInformer.Contracts;
using Codeinsight.VehicalInformer.Services;

namespace Codeinsight.VehicalInformer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IFileProcessor fileProcessor = new FileProcessor();
            IVehicalService vehicalService = new CarServices(fileProcessor);
            vehicalService.GetVehicalDetails();
        }
    }
}

