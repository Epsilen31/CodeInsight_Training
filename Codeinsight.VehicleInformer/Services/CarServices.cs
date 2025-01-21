using Codeinsight.VehicleInformer.Contracts;
using Codeinsight.VehicleInformer.DTOs;
using Codeinsight.VehicleInformer.Constants;
using Codeinsight.VehicleInformer.Enums;

namespace Codeinsight.VehicleInformer.Services
{
    public class CarServices : IVehicleService
    {
        private IFileProcessor FileProcessor { get; set; }
        public CarServices(IFileProcessor fileProcessor)
        {
             FileProcessor = fileProcessor;
        }

        public void GenerateVehicleReport()
        {
            try
            {
                var carsData = GetCarReportData();
                StoreCarsData(carsData);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
                Console.WriteLine($"Stack Trace: {exception.StackTrace}");
            }
        }

        public void DisplayVehicleReportInTabular()
        {
            try{
                var carsData = GetCarReportData();
                DisplayAllCars(carsData);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
                Console.WriteLine($"Stack Trace: {exception.StackTrace}");
            }
        }

        public ICollection<CarDto> SearchVehicleByModel()
        {
            try
            {
                var carsData = GetCarReportData();
                return GetSearchCarByModel(carsData);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
                Console.WriteLine($"Stack Trace: {exception.StackTrace}");
                return new List<CarDto>();
            }
        }

        public ICollection <CarDto> FilterVehiclesByManufacturingYear()
        {
            try
            {
                var carsData = GetCarReportData();
                return GetFilterCarsByManufacturingYear(carsData);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
                Console.WriteLine($"Stack Trace: {exception.StackTrace}");
                return new List<CarDto>();
            }
        }

        public ICollection <CarDto> SortVehiclesByPrice()
        {
            try
            {
                var carsData = GetCarReportData();
                return GetSortCarsByPrice(carsData);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
                Console.WriteLine($"Stack Trace: {exception.StackTrace}");
                return new List<CarDto>();
            }
        }

        public ICollection<AverageRatingDto> VehiclesAverageRating()
        {
            try
            {
                var carsData = GetCarReportData();
                return GetCarsAverageRating(carsData);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
                Console.WriteLine($"Stack Trace: {exception.StackTrace}");
                return new List<AverageRatingDto>();
            }
        }

        public ICollection <CarDto> CountVehiclesBasedOnRating()
        {
            try
            {
                var carsData = GetCarReportData();
                return GetCountCarsBasedOnRating(carsData);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
                Console.WriteLine($"Stack Trace: {exception.StackTrace}");
                return new List<CarDto>();
            }
        }

        private ICollection<CarDto> GetCarReportData(){
            return CarsReportData();
        }

        private ICollection<CarDto> CarsReportData()
        {
            string filePath = FilePathConstants.filePathValue;
            string carDetails = FileProcessor.ReadFiles(filePath);
            ICollection<CarDto> carsICollection =  ParseCarDetails(carDetails);
            return carsICollection;
        }

        private void StoreCarsData(ICollection<CarDto> cars)
        {
            string directoryPath = FilePathConstants.directoryPathValue;
            foreach (var car in cars)
            {
                string fileName = Path.Combine(directoryPath, $"{car.Company}_{car.Model}.txt");
                string carDetails = $"{TableHeaderConstants.Model}: {car.Model}\n" +
                                    $"{TableHeaderConstants.Company}: {car.Company}\n" +
                                    $"{TableHeaderConstants.ManufacturingYear}: {car.ManufacturingYear}\n" +
                                    $"{TableHeaderConstants.BasePrice}: {car.BasePrice}\n" +
                                    $"{TableHeaderConstants.InsurancePrice}: {car.InsurancePrice}\n" +
                                    $"{TableHeaderConstants.AfterTotalPrice}: {car.AfterTotalPrice}\n" +
                                    $"{TableHeaderConstants.Rating}: {car.Rating}\n";

                FileProcessor.GenerateFile(fileName, carDetails);
            }
        }

        private void DisplayAllCars(ICollection<CarDto> carDetails) {
            Console.WriteLine($"{TableHeaderConstants.Model}\t{TableHeaderConstants.Company}\t{TableHeaderConstants.ManufacturingYear}\t{TableHeaderConstants.BasePrice}\t{TableHeaderConstants.InsurancePrice}\t{TableHeaderConstants.AfterTotalPrice}\t{TableHeaderConstants.Rating}");
            foreach (var car in carDetails) {
                Console.WriteLine($"{car.Model}\t{car.Company}\t{car.ManufacturingYear}\t{car.BasePrice}\t{car.InsurancePrice}\t{car.AfterTotalPrice}\t{car.Rating} ");
            }
        }

        private ICollection<CarDto> GetSearchCarByModel(ICollection<CarDto> carDetails)
        {
            Console.WriteLine("Enter Car Model to search: ");
            string? carModel = Console.ReadLine();
            if (!string.IsNullOrEmpty(carModel))
            {
                return SearchCars(carModel, carDetails);
            }
            return new List<CarDto>();
        }

        private ICollection<CarDto> SearchCars(string carModel, ICollection<CarDto> carDetails)
        {
            carModel = carModel.ToLower();
            carDetails = carDetails.Where(car => car.Model.ToLower().Contains(carModel)).ToList();
            return carDetails;
        }

        private ICollection<CarDto> GetFilterCarsByManufacturingYear(ICollection<CarDto>carsData)
        {
            Console.WriteLine("Enter Manufacturing Year to filter: ");
            if (int.TryParse(Console.ReadLine(), out int manufacturingYear))
            {
                return CarsFiltering(manufacturingYear, carsData);
            }
            return new List<CarDto>();
        }

        private ICollection<CarDto> CarsFiltering(int manufacturingYear, ICollection<CarDto> carDetails)
        {
            carDetails = carDetails.Where(car => int.TryParse(car.ManufacturingYear, out int parsedYear) && parsedYear == manufacturingYear).ToList();
            return carDetails;
        }

        private ICollection<CarDto> GetSortCarsByPrice(ICollection<CarDto> carsData)
        {
            Console.WriteLine("Sort Cars by Price: ");
            Console.WriteLine("1. Base Price");
            Console.WriteLine("2. After Total Price");
            Console.WriteLine("Enter your choice: ");
            int sortChoice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("1. Ascending");
            Console.WriteLine("2. Descending");
            Console.WriteLine("Enter your choice: ");
            int sortOrder = Convert.ToInt32(Console.ReadLine());

            SortCriteria sortCriteria = (SortCriteria)sortChoice;
            SortOrder order = (SortOrder)sortOrder;

            return CarByPrice(sortCriteria, order, carsData);
        }

        private ICollection<CarDto> CarByPrice(SortCriteria sortCriteria, SortOrder sortOrder, ICollection<CarDto> carDetails)
        {
            switch (sortCriteria)
            {
                case SortCriteria.BasePrice:
                    carDetails = sortOrder == SortOrder.Ascending
                        ? carDetails.OrderBy(car => car.BasePrice).ToList()
                        : carDetails.OrderByDescending(car => car.BasePrice).ToList();
                    return carDetails;

                case SortCriteria.AfterTotalPrice:
                    carDetails = sortOrder == SortOrder.Ascending
                        ? carDetails.OrderBy(car => car.AfterTotalPrice).ToList()
                        : carDetails.OrderByDescending(car => car.AfterTotalPrice).ToList();
                    return carDetails;

                default:
                    Console.WriteLine("Invalid Sort Choice. Please choose either 1 or 2.");
                    return new List<CarDto>();
            }
        }

        private ICollection<AverageRatingDto> GetCarsAverageRating(ICollection<CarDto> carDetails)
        {
            ICollection<AverageRatingDto> averageRatingDtOs = GetAverageRating(carDetails);
            return averageRatingDtOs;
        }

        private ICollection <CarDto> GetCountCarsBasedOnRating(ICollection<CarDto> carDetails)
        {
            var ratingGroups = carDetails.GroupBy(car => car.Rating)
                                         .SelectMany(group => group)
                                         .ToList();
            return ratingGroups;
        }

        private ICollection<CarDto> ParseCarDetails(string carDetails)
        {
            var cars = new List<CarDto>();
            string[] carDetailLines = carDetails.Split("\n");

            for (int index = 1; index < carDetailLines.Length; index++)
            {
                var carDetail = carDetailLines[index].Split(",");

                if (carDetail.Length == 7)
                {
                    string model = string.IsNullOrEmpty(carDetail[0]) ? "Unknown Model" : carDetail[0];
                    string company = string.IsNullOrEmpty(carDetail[1]) ? "Unknown Company" : carDetail[1];
                    string manufacturingYear = string.IsNullOrEmpty(carDetail[2]) ? "Unknown Year" : carDetail[2];
                    string basePrice = string.IsNullOrEmpty(carDetail[3]) ? "0" : carDetail[3];
                    string insurancePrice = string.IsNullOrEmpty(carDetail[4]) ? "0" : carDetail[4];
                    string afterTotalPrice = string.IsNullOrEmpty(carDetail[5]) ? "0" : carDetail[5];
                    string rating = string.IsNullOrEmpty(carDetail[6]) ? "0" : carDetail[6];

                    var car = new CarDto
                    {
                        Model = model,
                        Company = company,
                        ManufacturingYear = manufacturingYear,
                        BasePrice = basePrice,
                        InsurancePrice = insurancePrice,
                        AfterTotalPrice = afterTotalPrice,
                        Rating = rating
                    };
                    cars.Add(car);
                }
            }
            return cars;
        }

        private ICollection<AverageRatingDto> GetAverageRating(ICollection<CarDto> carDetail)
        {
            ICollection<AverageRatingDto> averageRatingDtos = carDetail.GroupBy(car => car.Company)
                .Select(group => new AverageRatingDto
                {
                    Company = group.Key,
                    Rating = group.Average(car => Convert.ToDouble(car.Rating)).ToString("0.00")
                })
                .ToList();

            return averageRatingDtos;
        }
    }
}
