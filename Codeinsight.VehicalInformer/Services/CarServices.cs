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

                string directoryPath = FilePaths.directoryPathValue;
                storeCarsData(directoryPath , carsData);
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
            var carsList =  ParseCarDetails(carDetails);
            return carsList;
        }

        public void storeCarsData(string directoryPath, List<CarDTO> cars)
        {
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

        public static List<CarDTO> ParseCarDetails(string carDetails)
        {
            var cars = new List<CarDTO>();
            var carDetailLines = carDetails.Split("\n");

            foreach (var carDetailLine in carDetailLines)
            {
                var carDetail = carDetailLine.Split(",");

                if (carDetail.Length == 7)
                {
                    var model = string.IsNullOrEmpty(carDetail[0]) ? "Unknown Model" : carDetail[0];
                    var company = string.IsNullOrEmpty(carDetail[1]) ? "Unknown Company" : carDetail[1];
                    var manufacturingYear = string.IsNullOrEmpty(carDetail[2]) ? "Unknown Year" : carDetail[2];
                    var basePrice = string.IsNullOrEmpty(carDetail[3]) ? "0" : carDetail[3];
                    var insurancePrice = string.IsNullOrEmpty(carDetail[4]) ? "0" : carDetail[4];
                    var afterTotalPrice = string.IsNullOrEmpty(carDetail[5]) ? "0" : carDetail[5];
                    var rating = string.IsNullOrEmpty(carDetail[6]) ? "0" : carDetail[6];

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

