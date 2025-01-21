using Codeinsight.VehicleInformer.DTOs;
using Codeinsight.VehicleInformer.Contracts;
using Codeinsight.VehicleInformer.Constants;
using Codeinsight.VehicleInformer.Enums;

namespace Codeinsight.VehicleInformer.Services{

    public class CarsTaskManager : IVehicleTaskManager
    {
        private IVehicleService VehicleService { get; set; }
        public CarsTaskManager(IVehicleService vehicleService){
            VehicleService = vehicleService;
        }

        public void PerformVehicleTasks()
        {
            string operationString = VehicleOperationsAvailable();
            if (!Enum.TryParse(operationString, out VehicleOperations operation))
            {
                Console.WriteLine("Invalid Operation");
                return;
            }
            PerformVehicleOperation(operation, VehicleService);
        }

        private void PerformVehicleOperation(VehicleOperations operation, IVehicleService vehicleService)
        {
            switch (operation)
            {
                case VehicleOperations.GenerateVehicleReport:
                    vehicleService.GenerateVehicleReport();
                    break;

                case VehicleOperations.DisplayVehicleReportInTabular:
                    vehicleService.DisplayVehicleReportInTabular();
                    break;

                case VehicleOperations.SearchVehicleByModel:
                    ICollection<CarDto> carsModel = vehicleService.SearchVehicleByModel();
                    ShowCarsModel(carsModel);
                    break;

                case VehicleOperations.FilterVehiclesByManufacturingYear:
                    ICollection<CarDto> carsManufacturingYear = vehicleService.FilterVehiclesByManufacturingYear();
                    ShowCarsByManufacturingYear(carsManufacturingYear);
                    break;

                case VehicleOperations.SortVehiclesByPrice:
                    ICollection<CarDto> carsPrices = vehicleService.SortVehiclesByPrice();
                    ShowCarsByPrice(carsPrices);
                    break;

                case VehicleOperations.VehiclesAverageRating:
                    ICollection<AverageRatingDto> carsAverageRating = vehicleService.VehiclesAverageRating();
                    ShowCarsAvergedRating(carsAverageRating);
                    break;

                case VehicleOperations.CountVehiclesBasedOnRating:
                    ICollection<CarDto> carsCountRating = vehicleService.CountVehiclesBasedOnRating();
                    ShowCarsCountRating(carsCountRating);
                    break;

                default:
                    Console.WriteLine("Invalid Operation");
                    break;
            }
        }

        private string VehicleOperationsAvailable()
        {
            Console.WriteLine(VehicleConsoleOptionConstants.ChooseOperation);
            Console.WriteLine(VehicleConsoleOptionConstants.GenerateCarReport);
            Console.WriteLine(VehicleConsoleOptionConstants.DisplayAllCarsTabular);
            Console.WriteLine(VehicleConsoleOptionConstants.SearchCarByModel);
            Console.WriteLine(VehicleConsoleOptionConstants.FilterCarsByManufacturingYear);
            Console.WriteLine(VehicleConsoleOptionConstants.SortCarsByPrice);
            Console.WriteLine(VehicleConsoleOptionConstants.CarsAverageRating);
            Console.WriteLine(VehicleConsoleOptionConstants.CountCarsBasedOnRating);

            string operation = Console.ReadLine() ?? string.Empty;
            return operation;
        }

        private void ShowCarsModel(ICollection<CarDto> carsModel)
        {
            Console.WriteLine("Cars Model:");
            if (carsModel.Count > 0){
                Console.WriteLine($"{TableHeaderConstants.Model}\t{TableHeaderConstants.Company}\t{TableHeaderConstants.ManufacturingYear}\t{TableHeaderConstants.BasePrice}\t{TableHeaderConstants.InsurancePrice}\t{TableHeaderConstants.AfterTotalPrice}\t{TableHeaderConstants.Rating}");
                foreach (var car in carsModel)
                {
                    Console.WriteLine($"{car.Model}\t{car.Company}\t{car.ManufacturingYear}\t{car.BasePrice}\t{car.InsurancePrice}\t{car.AfterTotalPrice}\t{car.Rating}");
                }
            }
        }

        private void ShowCarsByManufacturingYear(ICollection<CarDto> carsManufacturingYear)
        {
            Console.WriteLine("Cars Manufacturing Year:");
            if (carsManufacturingYear.Count > 0){
               Console.WriteLine($"{TableHeaderConstants.Model}\t{TableHeaderConstants.Company}\t{TableHeaderConstants.ManufacturingYear}\t{TableHeaderConstants.BasePrice}\t{TableHeaderConstants.InsurancePrice}\t{TableHeaderConstants.AfterTotalPrice}\t{TableHeaderConstants.Rating}");
                foreach (var car in carsManufacturingYear)
                {
                    Console.WriteLine($"{car.Model}\t{car.Company}\t{car.ManufacturingYear}\t{car.BasePrice}\t{car.InsurancePrice}\t{car.AfterTotalPrice}\t{car.Rating}");
                }
            }
        }

        private void ShowCarsByPrice(ICollection<CarDto> carsPrices)
        {
            Console.WriteLine("Cars Sorted By Price:");
            if (carsPrices.Count > 0){
                Console.WriteLine($"{TableHeaderConstants.Model}\t{TableHeaderConstants.Company}\t{TableHeaderConstants.ManufacturingYear}\t{TableHeaderConstants.BasePrice}\t{TableHeaderConstants.InsurancePrice}\t{TableHeaderConstants.AfterTotalPrice}\t{TableHeaderConstants.Rating}");
                foreach (var car in carsPrices)
                {
                    Console.WriteLine($"{car.Model}\t{car.Company}\t{car.ManufacturingYear}\t{car.BasePrice}\t{car.InsurancePrice}\t{car.AfterTotalPrice}\t{car.Rating}");
                }
            }
        }

        private void ShowCarsAvergedRating(ICollection<AverageRatingDto> carsAverageRating)
        {
            Console.WriteLine("Cars Average Rating:");
            if (carsAverageRating.Count > 0){
                Console.WriteLine($"Company\tAverage Rating");
                foreach (var car in carsAverageRating)
                {
                    Console.WriteLine($"{car.Company}\t{car.Rating}");
                }
            }
            else
            {
                Console.WriteLine("No Car Data Available");
            }
            Console.WriteLine($"Total Records: {carsAverageRating.Count}\n");
        }

        private void ShowCarsCountRating(ICollection<CarDto> carsCountRating)
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
