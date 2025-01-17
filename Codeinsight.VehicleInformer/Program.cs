using Codeinsight.VehicleInformer.Contracts;
using Codeinsight.VehicleInformer.Services;

namespace Codeinsight.VehicleInformer
{
    public class Program
    {
        static void Main(string[] args)
        {
            IFileProcessor fileProcessor = new FileProcessor();
            IVehicleService vehicalService = new CarServices(fileProcessor);
            // vehicalService.GenerateCarReport();
            vehicalService.ConsoleCarReport();
        }
    }
}

