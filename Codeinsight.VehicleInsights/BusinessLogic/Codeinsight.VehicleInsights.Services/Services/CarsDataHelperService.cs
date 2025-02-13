using Codeinsight.VehicleInsights.Services.Contracts;
using Codeinsight.VehicleInsights.Services.DTOs;

namespace Codeinsight.VehicleInsights.Services.Services
{
    public class CarsDataHelperService : ICarsDataHelperServiceService
    {
        private readonly IFileHandler _fileHandler;

        public CarsDataHelperService(IFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }

        public async Task<ICollection<CarDto>> CarsReportCommonHelperAsyncAsync(
            string filePath,
            CancellationToken cancellationToken
        )
        {
            return await CarsReportDataAsync(filePath, cancellationToken);
        }

        private async Task<ICollection<CarDto>> CarsReportDataAsync(
            string filePath,
            CancellationToken cancellationToken
        )
        {
            string carDetails = await _fileHandler.ReadFileAsync(filePath, cancellationToken);
            ICollection<CarDto> carsCollection = ParseCarDetails(carDetails);
            return carsCollection;
        }

        private ICollection<CarDto> ParseCarDetails(string carDetails)
        {
            if (string.IsNullOrWhiteSpace(carDetails))
                return [];

            string[] carDetailLines = carDetails.Split("\n");
            var cars = new List<CarDto>();

            foreach (var line in carDetailLines.Skip(1))
            {
                var columns = line.Split(',');

                if (columns.Length != 7)
                    continue;

                var car = new CarDto
                {
                    Model = GetInfoItem(columns[0], "Unknown Model"),
                    Company = GetInfoItem(columns[1], "Unknown Company"),
                    ManufacturingYear = GetInfoItem(columns[2], "Unknown Year"),
                    BasePrice = GetInfoItem(columns[3], "0"),
                    InsurancePrice = GetInfoItem(columns[4], "0"),
                    AfterTotalPrice = GetInfoItem(columns[5], "0"),
                    Rating = GetInfoItem(columns[6], "0"),
                };
                cars.Add(car);
            }
            return cars;
        }

        private string GetInfoItem(string value, string defaultValue = "") =>
            !string.IsNullOrEmpty(value) ? value : defaultValue;
    }
}
