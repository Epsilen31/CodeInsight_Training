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

        public void GetVehicalDetails()
        {
            try
            {
                string filePath = FilePaths.filePathValue;
                string carDetails = FileProcessor.ReadCarDetails(filePath);
                var cars = ParseCarDetails(carDetails);

                WriteCarsToSeparateCSVs(cars);

                foreach (var car in cars)
                {
                    Console.WriteLine($"Model: {car.Model}, Company: {car.Company}, Manufacturing Year: {car.ManufacturingYear}, Base Price: {car.BasePrice}, Insurance Price: {car.InsurencePrice}, Total Price: {car.AfterTotalPrice}, Rating: {car.Rating}");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error", exception.Message);
            }
        }

        private List<CarDTO> ParseCarDetails(string carDetails)
        {
            var cars = new List<CarDTO>();
            var carDetailLines = carDetails.Split("\n");

            foreach (var carDetailLine in carDetailLines)
            {
                var carDetail = carDetailLine.Split(",");
                var car = new CarDTO
                {
                    Model = carDetail[0],
                    Company = carDetail[1],
                    ManufacturingYear = carDetail[2],
                    BasePrice = carDetail[3],
                    InsurencePrice = carDetail[4],
                    AfterTotalPrice = carDetail[5],
                    Rating = carDetail[6]
                };
                cars.Add(car);
            }
            return cars;
        }

        private void WriteCarsToSeparateCSVs(List<CarDTO> cars)
        {
            string directoryPath = @"E:\C#\Assignment\Codeinsight.VehicalInformer\Codeinsight.VehicalInformer\TextFiles";

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
                
                                    File.WriteAllText(fileName, carDetails);
            }
        }
    }
}

