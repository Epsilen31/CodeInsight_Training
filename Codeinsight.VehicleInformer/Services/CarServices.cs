using Codeinsight.VehicleInformer.Contracts;
using Codeinsight.VehicleInformer.DTOs;

namespace Codeinsight.VehicleInformer.Services
{
    internal class CarServices : IVehicleService
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

                // Store Cars Data in a different different files
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

                // Display All Cars in Tabular Format
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
                return[];
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
                return [];
            }
        }

        public ICollection <CarDto> SortVehiclesByPrice()
        {
            try
            {
                var carsData = GetCarReportData();
                // Sort Cars By Price
                return GetSortCarsByPrice(carsData);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
                Console.WriteLine($"Stack Trace: {exception.StackTrace}");
                return [];
            }
        }

        public ICollection<AverageRatingDto> VehiclesAvergeRating()
        {
            try
            {
                var carsData = GetCarReportData();

                return GetCarsAvergeRating(carsData);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
                Console.WriteLine($"Stack Trace: {exception.StackTrace}");
                return [];
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
                return [];
            }
        }

        public ICollection<CarDto> GetCarReportData(){
            return CarsReportData();
        }

        public ICollection<CarDto> CarsReportData()
        {
            string filePath = FilePaths.filePathValue;
            string carDetails = FileProcessor.ReadFiles(filePath);
            ICollection<CarDto> carsICollection =  ParseCarDetails(carDetails);
            return carsICollection;
        }

        public void StoreCarsData(ICollection<CarDto> cars)
        {
            string directoryPath = FilePaths.directoryPathValue;
            foreach (var car in cars)
            {
                string fileName = Path.Combine(directoryPath, $"{car.Company}_{car.Model}.txt");
                string carDetails = $"{TableHeader.Model}: {car.Model}\n" +
                                    $"{TableHeader.Company}: {car.Company}\n" +
                                    $"{TableHeader.ManufacturingYear}: {car.ManufacturingYear}\n" +
                                    $"{TableHeader.BasePrice}: {car.BasePrice}\n" +
                                    $"{TableHeader.InsurancePrice}: {car.InsurencePrice}\n" +
                                    $"{TableHeader.AfterTotalPrice}: {car.AfterTotalPrice}\n" +
                                    $"{TableHeader.Rating}: {car.Rating}\n";

                FileProcessor.GenerateFile(fileName, carDetails);
            }
        }

        public void DisplayAllCars(ICollection<CarDto> carDetails) {
            Console.WriteLine($"{TableHeader.Model}\t{TableHeader.Company}\t{TableHeader.ManufacturingYear}\t{TableHeader.BasePrice}\t{TableHeader.InsurancePrice}\t{TableHeader.AfterTotalPrice}\t{TableHeader.Rating}");
            foreach (var car in carDetails) {
                Console.WriteLine($"{car.Model}\t{car.Company}\t{car.ManufacturingYear}\t{car.BasePrice}\t{car.InsurencePrice}\t{car.AfterTotalPrice}\t{car.Rating} ");
            }
        }

        public ICollection<CarDto> GetSearchCarByModel(ICollection<CarDto> carDetails)
        { 
            Console.WriteLine("Enter Car Model to search: ");
            string? carModel = Console.ReadLine();
            if (!string.IsNullOrEmpty(carModel))
            {
                return SearchCars(carModel, carDetails);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid car model.");
                return [];
            }
        }

        public ICollection<CarDto> SearchCars(string carModel, ICollection<CarDto> carDetails)
        { 
            carModel = carModel.ToLower();
            carDetails = carDetails.Where(car => car.Model.ToLower().Contains(carModel)).ToList();
            return carDetails;
        }

        public ICollection<CarDto> GetFilterCarsByManufacturingYear(ICollection<CarDto>carsData)
        {
            Console.WriteLine("Enter Manufacturing Year to filter: ");
            if (int.TryParse(Console.ReadLine(), out int manufacturingYear))
            {
                return CarsFiltering(manufacturingYear, carsData);
             }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid manufacturing year.");
                return [];
            }
        }

        public ICollection<CarDto> CarsFiltering(int manufacturingYear, ICollection<CarDto> carDetails)
        {  
            carDetails = carDetails.Where(car => int.TryParse(car.ManufacturingYear, out int parsedYear) && parsedYear == manufacturingYear).ToList();
            return carDetails;
        }

        public static ICollection<CarDto> GetSortCarsByPrice(ICollection<CarDto> carsData)
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
            return CarByPrice(sortChoice, sortOrder, carsData);
        }

        public static ICollection<CarDto> CarByPrice(int sortChoice, int sortOrder, ICollection<CarDto> carDetails)
        {
            switch (sortChoice)
            { 
                case 1: 
                    carDetails = sortOrder == 1 
                        ? carDetails.OrderBy(car => car.BasePrice).ToList() 
                        : carDetails.OrderByDescending(car => car.BasePrice).ToList();
                    return carDetails;
                case 2:
                    carDetails = sortOrder == 1 
                        ? carDetails.OrderBy(car => car.AfterTotalPrice).ToList() 
                        : carDetails.OrderByDescending(car => car.AfterTotalPrice).ToList();
                    return carDetails;
                default:
                    Console.WriteLine("Invalid Sort Choice. Please choose either 1 or 2.");
                    return [] ;
            }
        }

        public ICollection<AverageRatingDto> GetCarsAvergeRating(ICollection<CarDto> carDetails)
        {   
            // generate average rating base in company
            ICollection<AverageRatingDto> averageRatingDTOs = GetAverageRating(carDetails);
            return averageRatingDTOs;
        }

        public ICollection <CarDto> GetCountCarsBasedOnRating(ICollection<CarDto> carDetails)
        { 
            var ratingGroups = carDetails.GroupBy(car => car.Rating)
                                         .SelectMany(group => group)
                                         .ToList();
            return ratingGroups;
        }

        public double GetAllCarsAvergeRating(ICollection<CarDto> carDetails){

                var ratings = carDetails.GroupBy(car => car.Rating );
                double totalRating = ratings.Sum(group => group.Count() * Convert.ToDouble(group.Key));
                double totalCars = carDetails.Count;
                double averageRating = totalRating / totalCars;
                return averageRating;
        }

        public static ICollection<CarDto> ParseCarDetails(string carDetails)
        {
            var cars = new List<CarDto>();
            string[] carDetailLines = carDetails.Split("\n");

            for (int i = 1; i < carDetailLines.Length; i++)
            {
                var carDetail = carDetailLines[i].Split(",");

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
                        InsurencePrice = insurancePrice,
                        AfterTotalPrice = afterTotalPrice,
                        Rating = rating
                    };
                    cars.Add(car);
                }
            }
            return cars;
        } 

        public static ICollection<AverageRatingDto> GetAverageRating(ICollection<CarDto> carDetail)
        {
            var averageRatingDtos = carDetail.GroupBy(car => car.Company)
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

