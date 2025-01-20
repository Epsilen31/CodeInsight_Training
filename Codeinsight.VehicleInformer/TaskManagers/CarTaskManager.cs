
using Codeinsight.VehicleInformer.DTOs;
using Codeinsight.VehicleInformer.Contracts;

namespace Codeinsight.VehicleInformer.TaskManagers{

    public class carsTaskManager : IVehicleTaskManager
    {   
        private IVehicleService VehicleService { get; set; }
        public carsTaskManager(IVehicleService vehicleService){
            VehicleService = vehicleService;
        }

        public void PerformVehicleTasks(){

            string operation = OperationsAvailable();
            PerformVehicleOperation(operation, VehicleService);
        } 

        public void PerformVehicleOperation(string operation, IVehicleService vehicleService)
        {
                switch (operation)
            {   
                case "1":
                    vehicleService.GenerateVehicleReport();
                    break;
                case "2":
                    vehicleService.DisplayVehicleReportInTabular();
                    break;
                case "3":
                    List<CarDto> carsModel =  vehicleService.SearchVehicleByModel();
                    ShowCarsModel(carsModel);
                    break;
                case "4":
                    List<CarDto> carsManufacturingYear = vehicleService.FilterVehiclesByManufacturingYear();
                    ShowCarsByManufacturingYear(carsManufacturingYear);
                    break;
                case "5":
                    List<CarDto> carsPrices = vehicleService.SortVehiclesByPrice();
                    ShowCarsByPrice(carsPrices);
                    break;
                case "6":
                    List<AverageRatingDto> carsAvergeRating = vehicleService.VehiclesAvergeRating();
                    ShowCarsAvergedRating(carsAvergeRating);
                    break;
                case "7":
                    List<CarDto> carsCountRating = vehicleService.CountVehiclesBasedOnRating();
                    ShowCarsCountRating(carsCountRating);
                    break;
                default:
                    Console.WriteLine("Invalid Operation");
                    break;
            }
        }

        public string OperationsAvailable()
        {
            Console.WriteLine("Choose an operation:");
            Console.WriteLine("1. for GenerateCarReport");
            Console.WriteLine("2. for Display All Cars in Tabular Format");
            Console.WriteLine("3. for Search Car By Model");
            Console.WriteLine("4. for Filter Cars By Manufacturing Year");
            Console.WriteLine("5. for Sort Cars By Price");
            Console.WriteLine("6. for Cars Average Rating");
            Console.WriteLine("7. for Count Cars Based On Rating");

            string operation = Console.ReadLine() ?? string.Empty;
            return operation;    
        }

        public void ShowCarsModel(List<CarDto> carsModel)
        { 
            Console.WriteLine("Cars Model:");
            if (carsModel.Count > 0){
                Console.WriteLine($"{TableHeader.Model}\t{TableHeader.Company}\t{TableHeader.ManufacturingYear}\t{TableHeader.BasePrice}\t{TableHeader.InsurancePrice}\t{TableHeader.AfterTotalPrice}\t{TableHeader.Rating}");
                foreach (var car in carsModel)
                {
                    Console.WriteLine($"{car.Model}\t{car.Company}\t{car.ManufacturingYear}\t{car.BasePrice}\t{car.InsurencePrice}\t{car.AfterTotalPrice}\t{car.Rating}");
                }
            }
        }

        public void ShowCarsByManufacturingYear(List<CarDto> carsManufacturingYear)
        { 
            Console.WriteLine("Cars Manufacturing Year:");
            if (carsManufacturingYear.Count > 0){
               Console.WriteLine($"{TableHeader.Model}\t{TableHeader.Company}\t{TableHeader.ManufacturingYear}\t{TableHeader.BasePrice}\t{TableHeader.InsurancePrice}\t{TableHeader.AfterTotalPrice}\t{TableHeader.Rating}");
                foreach (var car in carsManufacturingYear)
                {
                    Console.WriteLine($"{car.Model}\t{car.Company}\t{car.ManufacturingYear}\t{car.BasePrice}\t{car.InsurencePrice}\t{car.AfterTotalPrice}\t{car.Rating}");
                }
            }
        }

        public void ShowCarsByPrice(List<CarDto> carsPrices)
        { 
            Console.WriteLine("Cars Sorted By Price:");
            if (carsPrices.Count > 0){
                Console.WriteLine($"{TableHeader.Model}\t{TableHeader.Company}\t{TableHeader.ManufacturingYear}\t{TableHeader.BasePrice}\t{TableHeader.InsurancePrice}\t{TableHeader.AfterTotalPrice}\t{TableHeader.Rating}");
                foreach (var car in carsPrices)
                {
                    Console.WriteLine($"{car.Model}\t{car.Company}\t{car.ManufacturingYear}\t{car.BasePrice}\t{car.InsurencePrice}\t{car.AfterTotalPrice}\t{car.Rating}");
                }
            }
        }
        
        public void ShowCarsAvergedRating(List<AverageRatingDto> carsAvergeRating)
        {
            Console.WriteLine("Cars Average Rating:");
            if (carsAvergeRating.Count > 0){
                Console.WriteLine($"Company\tAverage Rating");
                foreach (var car in carsAvergeRating)
                {
                    Console.WriteLine($"{car.Company}\t{car.Rating}");
                }
            }
            else
            {
                Console.WriteLine("No Car Data Available");
            }
            Console.WriteLine($"Total Records: {carsAvergeRating.Count}\n");
        }

        public void ShowCarsCountRating(List<CarDto> carsCountRating)
        { 
            Console.WriteLine("Cars Count Based on Rating:");
            if (carsCountRating.Count > 0){
                var groupedByRating = carsCountRating.GroupBy(car => car.Rating);
                foreach (var ratingGroup in groupedByRating)
                {
                    Console.WriteLine($"Rating: {ratingGroup.Key} Count: {ratingGroup.Count()}");
                }
                Console.WriteLine($"Average Rating: {carsCountRating.Average(car => double.Parse(car.Rating))}\n");
            }
        }
    }
}
