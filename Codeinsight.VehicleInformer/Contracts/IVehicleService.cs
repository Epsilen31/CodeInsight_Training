using Codeinsight.VehicleInformer.DTOs;

namespace Codeinsight.VehicleInformer.Contracts
{
    public interface IVehicleService
    {
        void GenerateVehicleReport();
        void DisplayVehicleReportInTabular();
        List <CarDto> SearchVehicleByModel();
        List <CarDto> FilterVehiclesByManufacturingYear();
        List <CarDto> SortVehiclesByPrice();
        List<AverageRatingDto> VehiclesAvergeRating();
        List <CarDto> CountVehiclesBasedOnRating();
    }
}

