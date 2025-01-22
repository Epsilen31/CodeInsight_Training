using Codeinsight.VehicleInformer.Contracts;
using Codeinsight.VehicleInformer.Services;

namespace Codeinsight.VehicleInformer
{
    public class Program
    {
        static void Main(string[] args)
        {
            IFileProcessor fileProcessor = new FileProcessor();
            IVehicleService vehicleService = new CarServices(fileProcessor);
            IVehicleTaskManager vehicleTaskManager = new CarsTaskManager(vehicleService);
            vehicleTaskManager.PerformVehicleTasks();
        }
    }
}
