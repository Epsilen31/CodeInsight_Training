using Codeinsight.VehicleInformer.Contracts;
using Codeinsight.VehicleInformer.Services;
using Codeinsight.VehicleInformer.TaskManagers;

namespace Codeinsight.VehicleInformer
{
    public class Program
    {
        static void Main(string[] args)
        {
            IFileProcessor fileProcessor = new FileProcessor();
            IVehicleService vehicleService = new CarServices(fileProcessor);
            IVehicleTaskManager vehicalTaskManager = new carsTaskManager(vehicleService);
            vehicalTaskManager.PerformVehicleTasks();          
        } 
    }
}

