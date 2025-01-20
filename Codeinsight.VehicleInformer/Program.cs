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
            vehicalService.GenerateCarReport();
            vehicalService.ConsoleCarReport(OperationsPerformation());
        }

        public static string OperationsPerformation()
        {
            Console.WriteLine("Choose an operation:");
            Console.WriteLine("1. for Display All Cars");
            Console.WriteLine("2. for Search Car By Model");
            Console.WriteLine("3. for Filter Cars By Manufacturing Year");
            Console.WriteLine("4. for Sort Cars By Price");
            Console.WriteLine("5. for Cars Average Rating");
            Console.WriteLine("6. for Count Cars Based On Rating");
            Console.WriteLine("7. for Export Report");

            string operation = Console.ReadLine() ?? string.Empty;
            return operation;    
        }
    }
}


