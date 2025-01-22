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
            PerformVehicleOperation(operation);
        }

        private void PerformVehicleOperation(VehicleOperations operation)
        {
            switch (operation)
            {
                case VehicleOperations.GenerateVehicleReport:
                    VehicleService.GenerateVehicleReport();
                    break;

                case VehicleOperations.DisplayVehicleReportInTabular:
                    VehicleService.DisplayVehicleReportInTabular();
                    break;

                case VehicleOperations.SearchVehicleByModel:
                    SearchVehiclesByModel();
                    break;

                case VehicleOperations.FilterVehiclesByManufacturingYear:
                    FilterVehiclesByManufacturingYear();
                    break;

                case VehicleOperations.SortVehiclesByPrice:
                    SortVehiclesByPrice();
                    break;

                case VehicleOperations.VehiclesAverageRating:
                    GetVehiclesAverageRating();
                    break;

                case VehicleOperations.CountVehiclesBasedOnRating:
                    GetVehicleCountByRating();
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

        private void SearchVehiclesByModel()
        {
            ICollection<CarDto> carsModel = VehicleService.SearchVehicleByModel();
            DisplayCarsByModel(carsModel);
        }

        private void FilterVehiclesByManufacturingYear()
        {
            ICollection<CarDto> carsManufacturingYear = VehicleService.GetFilterVehiclesByManufacturingYear();
            DisplayCarsByManufacturingYear(carsManufacturingYear);
        }

        private void SortVehiclesByPrice()
        {
            ICollection<CarDto> carsPrices = VehicleService.GetSortedVehiclesByPrice();
            DisplayCarsByPrice(carsPrices);
        }

        private void GetVehiclesAverageRating()
        {
            ICollection<AverageRatingDto> carsAverageRating = VehicleService.GetVehiclesAverageRating();
            DisplayCarsAverageRating(carsAverageRating);
        }

        private void GetVehicleCountByRating()
        {
            ICollection<CarDto> carsCountRating = VehicleService.GetVehiclesCountBasedOnRating();
            DisplayCarsCountByRating(carsCountRating);
        }

        private void DisplayCarsByModel(ICollection<CarDto> carsModel)
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

        private void DisplayCarsByManufacturingYear(ICollection<CarDto> carsManufacturingYear)
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

        private void DisplayCarsByPrice(ICollection<CarDto> carsPrices)
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

        private void DisplayCarsAverageRating(ICollection<AverageRatingDto> carsAverageRating)
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

        private void DisplayCarsCountByRating(ICollection<CarDto> carsCountRating)
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
