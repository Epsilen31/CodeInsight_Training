using Codeinsight.VehicalInformer.Contracts;
using Codeinsight.VehicalInformer.DTOs;

namespace Codeinsight.VehicalInformer.Services
{
    internal class CarServices : IVehicalService
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
                var carsData = GetCarsReportData();

                // Store Cars Data in a different different files
                StoreCarsData(carsData);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
                Console.WriteLine($"Stack Trace: {exception.StackTrace}");
            }
        }

        public void ConsoleCarReport(){

            try{
                var carsData = GetCarsReportData();

                // Display All Cars in Tabular Form
                DisplayAllCars(carsData);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
                Console.WriteLine($"Stack Trace: {exception.StackTrace}");
            }   
        }

        public List<CarDTO> GetCarsReportData()
        {
            string filePath = FilePaths.filePathValue;
            string carDetails = FileProcessor.ReadFiles(filePath);
            List<CarDTO> carsList =  ParseCarDetails(carDetails);
            return carsList;
        }

        public void StoreCarsData(List<CarDTO> cars)
        {
            string directoryPath = FilePaths.directoryPathValue;
        
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            foreach (var car in cars)
            {
                string fileName = @$"{directoryPath}\{car.Company}_{car.Model}.txt";
                string carDetails = $"Model: {car.Model}\n" +
                                    $"Company: {car.Company}\n" +
                                    $"Manufacturing Year: {car.ManufacturingYear}\n" +
                                    $"Base Price: {car.BasePrice:C}\n" +
                                    $"Insurance Price: {car.InsurencePrice:C}\n" +
                                    $"After Total Price: {car.AfterTotalPrice:C}\n" +
                                    $"Rating: {car.Rating}/5\n";

                FileProcessor.GenerateFile(fileName, carDetails);
            }
        }

        public void DisplayAllCars(List<CarDTO> carDetails) {
            foreach (var car in carDetails) {
                Console.WriteLine($"{car.Model}\t{car.Company}\t{car.ManufacturingYear}\t{car.BasePrice}\t{car.InsurencePrice}\t{car.AfterTotalPrice}\t{car.Rating} ");
            }
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

