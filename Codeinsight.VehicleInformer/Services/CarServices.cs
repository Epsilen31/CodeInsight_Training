using System.Text;
using Codeinsight.VehicleInformer.Contracts;
using Codeinsight.VehicleInformer.DTOs;
using Microsoft.VisualBasic;

namespace Codeinsight.VehicleInformer.Services
{
    internal class CarServices : IVehicleService
    {
        private IFileProcessor FileProcessor { get; set; }
        public CarServices(IFileProcessor fileProcessor)
        {
             FileProcessor = fileProcessor;
        }
        
        public void GenerateCarReport()
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

        public void ConsoleCarReport(string operation)
        {
            try{
                
                var carsData = GetCarReportData();
            
                switch (operation)
                { 
                    case "1":
                        DisplayAllCars(carsData);
                        break;
                    case "2":
                        SearchCarByModel(carsData);
                        break;
                    case "3":
                        FilterCarsByManufacturingYear(carsData);
                        break;
                    case "4":
                        SortCarsByPrice(carsData);
                        break;
                    case "5":
                        CarsAvergeRating(carsData);
                        break;
                    case "6":
                        CountCarsBasedOnRating(carsData);
                        break;
                    case "7":
                        GenerateSummaryReport(carsData);
                        break;
                    default:
                        Console.WriteLine("Invalid Operation. Please choose a valid operation.");
                        break;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
                Console.WriteLine($"Stack Trace: {exception.StackTrace}");
            }   
        }

        public List<CarDTO> GetCarReportData(){
            return CarsReportData();
        }

        public List<CarDTO> CarsReportData()
        {
            string filePath = FilePaths.filePathValue;
            string carDetails = FileProcessor.ReadFiles(filePath);
            List<CarDTO> carsList =  ParseCarDetails(carDetails);
            return carsList;
        }

        public void StoreCarsData(List<CarDTO> cars)
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

        public void DisplayAllCars(List<CarDTO> carDetails) {
            Console.WriteLine($"{TableHeader.Model}\t{TableHeader.Company}\t{TableHeader.ManufacturingYear}\t{TableHeader.BasePrice}\t{TableHeader.InsurancePrice}\t{TableHeader.AfterTotalPrice}\t{TableHeader.Rating}");
            foreach (var car in carDetails) {
                Console.WriteLine($"{car.Model}\t{car.Company}\t{car.ManufacturingYear}\t{car.BasePrice}\t{car.InsurencePrice}\t{car.AfterTotalPrice}\t{car.Rating} ");
            }
        }

        public void SearchCarByModel(List<CarDTO> carDetails)
        { 
            Console.WriteLine("Enter Car Model to search: ");
            string? carModel = Console.ReadLine();
            if (!string.IsNullOrEmpty(carModel))
            {
                SearchCars(carModel, carDetails);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid car model.");
            }
        }

        public void SearchCars(string carModel, List<CarDTO> carDetails)
        { 
            carModel = carModel.ToLower();
            carDetails = carDetails.Where(car => car.Model.ToLower().Contains(carModel)).ToList();
            if (carDetails.Count > 0){
                Console.WriteLine($"{TableHeader.Model}\t{TableHeader.Company}\t{TableHeader.ManufacturingYear}\t{TableHeader.BasePrice}\t{TableHeader.InsurancePrice}\t{TableHeader.AfterTotalPrice}\t{TableHeader.Rating}");
                foreach (var car in carDetails)
                {
                    Console.WriteLine($"{car.Model}\t{car.Company}\t{car.ManufacturingYear}\t{car.BasePrice}\t{car.InsurencePrice}\t{car.AfterTotalPrice}\t{car.Rating}");
                }
            }
        }

        public void FilterCarsByManufacturingYear(List<CarDTO>carsData)
        {
            Console.WriteLine("Enter Manufacturing Year to filter: ");
            if (int.TryParse(Console.ReadLine(), out int manufacturingYear))
            {
                CarsFiltering(manufacturingYear, carsData);
             }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid manufacturing year.");
            }
        }

        public void CarsFiltering(int manufacturingYear, List<CarDTO> carDetails)
        {  
            // impliment using LINQ
            carDetails = carDetails.Where(car => int.TryParse(car.ManufacturingYear, out int parsedYear) && parsedYear == manufacturingYear).ToList();
            if (carDetails.Count > 0){
               Console.WriteLine($"{TableHeader.Model}\t{TableHeader.Company}\t{TableHeader.ManufacturingYear}\t{TableHeader.BasePrice}\t{TableHeader.InsurancePrice}\t{TableHeader.AfterTotalPrice}\t{TableHeader.Rating}");
                foreach (var car in carDetails)
                {
                    Console.WriteLine($"{car.Model}\t{car.Company}\t{car.ManufacturingYear}\t{car.BasePrice}\t{car.InsurencePrice}\t{car.AfterTotalPrice}\t{car.Rating}");
                }
            }
        }

        public static void SortCarsByPrice(List<CarDTO> carsData)
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
            CarByPrice(sortChoice, sortOrder, carsData);
        }

        public static void CarByPrice(int sortChoice, int sortOrder, List<CarDTO> carDetails)
        {
            switch (sortChoice)
            { 
                case 1: 
                    carDetails = sortOrder == 1 
                        ? carDetails.OrderBy(car => car.BasePrice).ToList() 
                        : carDetails.OrderByDescending(car => car.BasePrice).ToList();
                    break;
                case 2:
                    carDetails = sortOrder == 1 
                        ? carDetails.OrderBy(car => car.AfterTotalPrice).ToList() 
                        : carDetails.OrderByDescending(car => car.AfterTotalPrice).ToList();
                    break;
                default:
                    Console.WriteLine("Invalid Sort Choice. Please choose either 1 or 2.");
                    return;
            }
        }

        public double CarsAvergeRating(List<CarDTO> carDetails)
        {
            double totalRating = 0;
            foreach (var car in carDetails)
            {
                totalRating += Convert.ToDouble(car.Rating);
            }
            double averageRating = totalRating / carDetails.Count;
            Console.WriteLine($"Average Rating of all cars: {averageRating}");
            return averageRating;
        }

        public void CountCarsBasedOnRating(List<CarDTO> carDetails)
        { 
            var ratingGroups = carDetails.GroupBy(car => car.Rating);
            foreach (var ratingGroup in ratingGroups)
            {
                Console.WriteLine($"Rating: {ratingGroup.Key} Count: {ratingGroup.Count()}");
            }
            Console.WriteLine($"Average Rating: {CarsAvergeRating(carDetails)}\n");
        }

        public void GenerateSummaryReport(List<CarDTO> carDetails)
        {   
            
            string filePath = FilePaths.summaryReportPathValue;
            double averageRating = CarsAvergeRating(carDetails);

            StringBuilder summaryReport = new();
            summaryReport.AppendLine($"Total Cars: {carDetails.Count}");
            summaryReport.AppendLine($"Average Rating: {averageRating:F2}");
            summaryReport.AppendLine("\nCar Details:");
            summaryReport.AppendLine($"{TableHeader.Model}\t{TableHeader.Company}\t{TableHeader.BasePrice}\t{TableHeader.Rating}");

            foreach (var car in carDetails)
            {
                summaryReport.AppendLine($"{car.Model}\t{car.Company}\t{car.BasePrice}\t{car.Rating}");
            }
            FileProcessor.GenerateFile(filePath, summaryReport.ToString());
        }

        public static List<CarDTO> ParseCarDetails(string carDetails)
        {
            var cars = new List<CarDTO>();
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

                    var car = new CarDTO
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

        
    }
}

