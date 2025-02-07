using Codeinsight.VehicleInsights.Services.Contracts;
using Codeinsight.VehicleInsights.Services.DTOs;

namespace Codeinsight.VehicleInsights.Services.Services
{
    public class CarsDataHelper : ICarsDataHelper
    {
        private readonly IFileHandler _fileHandler;

        public CarsDataHelper(IFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }

        public async Task<ICollection<CarDto>> CarsReportCommonHelperAsyncAsync(
            string filePath,
            CancellationToken token
        )
        {
            return await GetCarReportDataAsync(filePath, token);
        }

        public async Task<ICollection<CarDto>> GetCarReportDataAsync(
            string filePath,
            CancellationToken token
        )
        {
            return await CarsReportDataAsync(filePath, token);
        }

        private async Task<ICollection<CarDto>> CarsReportDataAsync(
            string filePath,
            CancellationToken token
        )
        {
            string carDetails = await _fileHandler.ReadFileAsync(filePath, token);
            ICollection<CarDto> carsCollection = await ParseCarDetailsAsync(carDetails);
            return carsCollection;
        }

        private async Task<ICollection<CarDto>> ParseCarDetailsAsync(string carDetails)
        {
            return await Task.Run(() =>
            {
                var cars = new List<CarDto>();
                string[] carDetailLines = carDetails.Split("\n");

                for (int index = 1; index < carDetailLines.Length; index++)
                {
                    var carDetail = carDetailLines[index].Split(",");

                    if (carDetail.Length == 7)
                    {
                        string model = string.IsNullOrEmpty(carDetail[0])
                            ? "Unknown Model"
                            : carDetail[0];
                        string company = string.IsNullOrEmpty(carDetail[1])
                            ? "Unknown Company"
                            : carDetail[1];
                        string manufacturingYear = string.IsNullOrEmpty(carDetail[2])
                            ? "Unknown Year"
                            : carDetail[2];
                        string basePrice = string.IsNullOrEmpty(carDetail[3]) ? "0" : carDetail[3];
                        string insurancePrice = string.IsNullOrEmpty(carDetail[4])
                            ? "0"
                            : carDetail[4];
                        string afterTotalPrice = string.IsNullOrEmpty(carDetail[5])
                            ? "0"
                            : carDetail[5];
                        string rating = string.IsNullOrEmpty(carDetail[6]) ? "0" : carDetail[6];

                        var car = new CarDto
                        {
                            Model = model,
                            Company = company,
                            ManufacturingYear = manufacturingYear,
                            BasePrice = basePrice,
                            InsurancePrice = insurancePrice,
                            AfterTotalPrice = afterTotalPrice,
                            Rating = rating,
                        };
                        cars.Add(car);
                    }
                }
                return cars;
            });
        }
    }
}
